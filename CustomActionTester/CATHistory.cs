using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Rappen.XTB.CAT
{
    using ParamType = Rappen.XTB.CAT.Customapirequestparameter.Type_OptionSet;

    public partial class CustomActionTester
    {
        private List<CATRequest> histories;
        private CATRequest reloadhistoryrequest;

        private void LoadAndShowHistoryIfNeeded()
        {
            panHistoryOpen1.Visible = splitToolHistory.Panel2Collapsed;
            panHistoryOpen2.Visible = splitToolHistory.Panel2Collapsed;
            if (!splitToolHistory.Panel2Collapsed)
            {
                ShowHistory();
            }
        }

        private void AddHistoryToList(CATRequest catreq)
        {
            catreq.Organization = new Organization
            {
                ConnectionName = ConnectionDetail.ConnectionName,
                Unique = ConnectionDetail.Organization,
                Name = ConnectionDetail.OrganizationFriendlyName,
                WebAddress = ConnectionDetail.WebApplicationUrl
            };
            if (cmbSolution.SelectedRecord is Entity solution)
            {
                catreq.Solution = new SolutionInfo
                {
                    Unique = solution.TryGetAttributeValue("uniquename", out string solunique) ? solunique : null,
                    Name = solution.TryGetAttributeValue("friendlyname", out string solname) ? solname : null
                };
            }
            catreq.Name = cmbCustomActions.SelectedRecord?.TryGetAttributeValue(catTool.Columns.APIName, out string caname) == true ? caname : null;
            histories.Insert(0, catreq);
            SaveHistoryToFile();
            ShowHistory();
        }

        private void SaveHistoryToFile()
        {
            SettingsManager.Instance.Save(typeof(CustomActionTester), histories, "History");
        }

        private void ShowHistory()
        {
            var friendly = mnuShowDisplay.Checked;
            if (histories == null || histories.Count == 0)
            {
                GetHistoryFromFile();
            }
            listHistory.Items.Clear();
            listHistory.Groups.Clear();
            if (histories == null || histories.Count == 0)
            {
                return;
            }
            listHistory.ShowGroups = !rbHistGroupNone.Checked;
            var timeformat = rbHistGroupDate.Checked ? "T" : "G";
            colTime.Text = rbHistGroupDate.Checked ? "Time" : "Date";
            var showinghistories = histories.Where(h => !chkHistOnlyThisEnv.Checked || h.Organization?.ConnectionName == ConnectionDetail?.ConnectionName);
            if (rbHistGroupDate.Checked)
            {
                listHistory.Groups.AddRange(showinghistories
                    .OrderBy(h => h.Execution.RunTime)
                    .Reverse()
                    .Select(h => h.Execution.RunTime.Date)
                    .Distinct()
                    .Select(d => new ListViewGroup(d.ToString(), d.ToString("d"))).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(showinghistories
                       .Where(h => h.Execution.RunTime.Date.Equals(DateTime.Parse(g.Name)))
                       .Select(h => h.GetListItem(g, timeformat, friendly)).ToArray()));
            }
            else if (rbHistGroupSolution.Checked)
            {
                listHistory.Groups.AddRange(showinghistories
                    .OrderBy(h => h.Solution?.Name)
                    .Select(h => new Tuple<string, string>(h.Solution?.Unique, h.Solution?.Name))
                    .Distinct()
                    .Select(s => new ListViewGroup(s.Item1.ToString(), s.Item2)).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(showinghistories
                       .Where(h => h.Solution?.Unique == g.Name)
                       .Select(h => h.GetListItem(g, timeformat, friendly)).ToArray()));
            }
            else if (rbHistGroupAPI.Checked)
            {
                listHistory.Groups.AddRange(showinghistories
                    .OrderBy(h => h.Name)
                    .Select(h => new Tuple<string, string>(h.UniqueName, h.Name))
                    .Distinct()
                    .Select(s => new ListViewGroup(s.Item1.ToString(), s.Item2)).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(showinghistories
                       .Where(h => h.UniqueName.Equals(g.Name))
                       .Select(h => h.GetListItem(g, timeformat, friendly)).ToArray()));
            }
            else
            {
                listHistory.Items.AddRange(showinghistories
                    .Select(h => h.GetListItem(null, timeformat, friendly)).ToArray());
            }
            listHistory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (chkHistOnlyThisEnv.Checked)
            {
                listHistory.Columns[colEnv.Index].Width = 0;
            }
            if (rbHistGroupSolution.Checked)
            {
                listHistory.Columns[colSolution.Index].Width = 0;
            }
            if (rbHistGroupAPI.Checked)
            {
                listHistory.Columns[colAPI.Index].Width = 0;
            }
            if (!showinghistories.Any(h => !string.IsNullOrEmpty(h.Execution?.ErrorMessage)))
            {
                listHistory.Columns[colError.Index].Width = 0;
            }
            EnableHistButtons();
        }

        private void GetHistoryFromFile()
        {
            if (SettingsManager.Instance.TryLoad(typeof(CustomActionTester), out List<CATRequest> fromfilehistory, "History"))
            {
                histories = fromfilehistory;
            }
            else
            {
                histories = new List<CATRequest>();
            }
            histories = histories.OrderBy(req => req.Execution?.RunTime).Reverse().ToList();
        }

        private List<CATRequest> GetHistoryFromList()
        {
            return listHistory.Items.Cast<ListViewItem>()
                .Where(l => l.Tag is CATRequest)
                .Select(l => l.Tag as CATRequest)
                .ToList();
        }

        private void DeleteHistories(List<CATRequest> deletehistories)
        {
            if (deletehistories.Count > 1 &&
                MessageBox.Show($"Confirm deleting these {deletehistories.Count} histories.", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            {
                return;
            }
            deletehistories.ForEach(d => histories.Remove(d));
            SaveHistoryToFile();
            ShowHistory();
        }

        private void EnableHistButtons()
        {
            btnHistReload.Enabled = listHistory.SelectedItems.Count == 1;
            btnHistDelete.Enabled = listHistory.SelectedItems.Count > 0;
            btnHistDeleteAll.Enabled = listHistory.Items.Count > 0;
        }

        private void ReloadHistoryItem(CATRequest history)
        {
            reloadhistoryrequest = history;
            ReloadHistoryItem(0);
        }

        private void ReloadHistoryItem(int nextstepnum)
        {
            switch (nextstepnum)
            {
                case 0:     // Select correct Solution Managed Type, load Solutions
                    if (GetSolutionType() == SolutionType.All)
                    {
                        ReloadHistoryItem(++nextstepnum);
                    }
                    else
                    {
                        GetSolutions(SolutionType.All, ReloadHistoryItem, ++nextstepnum);
                    }
                    break;

                case 1:     // Select currect Solution, load Custon Actions
                    if (SelectedSolutionUnique == reloadhistoryrequest.Solution?.Unique)
                    {
                        ReloadHistoryItem(++nextstepnum);
                    }
                    else
                    {
                        GetCustomActions(reloadhistoryrequest.Solution?.Unique, ReloadHistoryItem, ++nextstepnum);
                    }
                    break;

                case 2:
                    if (SelectedSolutionUnique != reloadhistoryrequest.Solution?.Unique)
                    {
                        GetCustomActions("Default", ReloadHistoryItem, ++nextstepnum);
                    }
                    else
                    {
                        ReloadHistoryItem(++nextstepnum);
                    }
                    break;

                case 3:
                    if (SelectedCustomUnique != reloadhistoryrequest.UniqueName)
                    {
                        SelectCmbByStringAttribute(cmbCustomActions, catTool.Columns.APIUniqueName, reloadhistoryrequest.UniqueName);
                        SetCustomAction(cmbCustomActions.SelectedRecord);
                    }
                    ReloadHistoryItem(++nextstepnum);
                    break;

                case 4:
                    if (SelectedCustomUnique != reloadhistoryrequest.UniqueName)
                    {
                        Enabled = true;
                        MessageBox.Show($"Can't find the {catTool.Target}.", "Reload", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    GetInputParams(cmbCustomActions.SelectedRecord?.Id ?? Guid.Empty, ReloadHistoryItem, ++nextstepnum);
                    break;

                case 5:
                    SetInputParametersValues(reloadhistoryrequest.Parameters);
                    LogUse("Reload");
                    Enabled = true;
                    break;
            }
        }

        private void SetInputParametersValues(List<CATParameter> parameters)
        {
            var inputs = gridInputParams.GetDataSource<IEnumerable<Entity>>();
            if (inputs == null)
            {
                return;
            }
            foreach (var param in parameters)
            {
                var input = inputs.FirstOrDefault(i => i.TryGetAttributeValue(catTool.Columns.ParamUniqueName, out string name) && name == param.Name);
                if (input == null)
                {
                    continue;
                }
                switch (param.Type)
                {
                    case ParamType.Entity:
                        var entity = param.RawValue as Entity;
                        entity = Service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));
                        input["rawvalue"] = entity;
                        input["value"] = GetEntityPrimaryName(entity);
                        break;

                    case ParamType.EntityReference:
                        var entityref = param.RawValue as EntityReference;
                        entity = Service.Retrieve(entityref.LogicalName, entityref.Id, new ColumnSet(true));
                        input["value"] = GetEntityPrimaryName(entity);
                        break;

                    default:
                        input["rawvalue"] = param.RawValue;
                        input["value"] = param.Value;
                        break;
                }
                RefreshInputParameters(input);
            }
        }
    }
}
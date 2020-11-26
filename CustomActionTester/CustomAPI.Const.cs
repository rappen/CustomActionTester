// *********************************************************************
// Created by : Latebound Constants Generator 1.2020.2.1 for XrmToolBox
// Author     : Jonas Rapp https://twitter.com/rappen
// GitHub     : https://github.com/rappen/LCG-UDG
// Source Org : https://jonassandbox.crm4.dynamics.com/
// Filename   : C:\Dev\GitHub\CustomActionTester\CustomActionTester\CustomAPI.Const.cs
// Created    : 2020-11-26 23:43:34
// *********************************************************************

namespace Rappen.XTB.CAT
{
    /// <summary>DisplayName: Custom API, OwnershipType: UserOwned, IntroducedVersion: 1.0.0.0</summary>
    public static class Customapi
    {
        public const string EntityName = "customapi";
        public const string EntityCollectionName = "customapis";

        #region Attributes

        /// <summary>Type: Uniqueidentifier, RequiredLevel: SystemRequired</summary>
        public const string PrimaryKey = "customapiid";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 100, Format: Text</summary>
        public const string PrimaryName = "name";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Allowed Custom Processing Step Type, OptionSetType: Picklist, DefaultFormValue: 0</summary>
        public const string AllowedcustomProcessingStepType = "allowedcustomprocessingsteptype";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Binding Type, OptionSetType: Picklist, DefaultFormValue: 0</summary>
        public const string BindingType = "bindingtype";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string BoundEntityLogicalName = "boundentitylogicalname";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Component State, OptionSetType: Picklist</summary>
        public const string ComponentState = "componentstate";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: systemuser</summary>
        public const string CreatedBy = "createdby";
        /// <summary>Type: DateTime, RequiredLevel: None, Format: DateAndTime, DateTimeBehavior: UserLocal</summary>
        public const string CreatedOn = "createdon";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 100, Format: Text</summary>
        public const string Description = "description";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 100, Format: Text</summary>
        public const string DisplayName = "displayname";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string ExecuteprivilegeName = "executeprivilegename";
        /// <summary>Type: ManagedProperty, RequiredLevel: SystemRequired</summary>
        public const string Iscustomizable = "iscustomizable";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Isfunction = "isfunction";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Ismanaged = "ismanaged";
        /// <summary>Type: Boolean, RequiredLevel: None, True: 1, False: 0, DefaultValue: False</summary>
        public const string IsPrivate = "isprivate";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: systemuser</summary>
        public const string ModifiedBy = "modifiedby";
        /// <summary>Type: DateTime, RequiredLevel: None, Format: DateAndTime, DateTimeBehavior: UserLocal</summary>
        public const string ModifiedOn = "modifiedon";
        /// <summary>Type: Owner, RequiredLevel: SystemRequired, Targets: systemuser,team</summary>
        public const string OwnerId = "ownerid";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: plugintype</summary>
        public const string PluginTypeId = "plugintypeid";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: sdkmessage</summary>
        public const string SdkMessageId = "sdkmessageid";
        /// <summary>Type: State, RequiredLevel: SystemRequired, DisplayName: Status, OptionSetType: State</summary>
        public const string StateCode = "statecode";
        /// <summary>Type: Status, RequiredLevel: None, DisplayName: Status Reason, OptionSetType: Status</summary>
        public const string StatusCode = "statuscode";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 128, Format: Text</summary>
        public const string UniqueName = "uniquename";

        #endregion Attributes

        #region Relationships

        /// <summary>Parent: "Customapi" Child: "Customapirequestparameter" Lookup: "CustomapiId"</summary>
        public const string Rel1M_CustomapiCustomapirequestparameter = "customapi_customapirequestparameter";
        /// <summary>Parent: "Customapi" Child: "Customapiresponseproperty" Lookup: "CustomapiId"</summary>
        public const string Rel1M_CustomapiCustomapiresponseproperty = "customapi_customapiresponseproperty";

        #endregion Relationships

        #region OptionSets

        public enum AllowedcustomProcessingStepType_OptionSet
        {
            None = 0,
            AsyncOnly = 1,
            SyncandAsync = 2
        }
        public enum BindingType_OptionSet
        {
            Global = 0,
            Entity = 1,
            EntityCollection = 2
        }
        public enum ComponentState_OptionSet
        {
            Published = 0,
            Unpublished = 1,
            Deleted = 2,
            DeletedUnpublished = 3
        }
        public enum StateCode_OptionSet
        {
            Active = 0,
            Inactive = 1
        }
        public enum StatusCode_OptionSet
        {
            Active = 1,
            Inactive = 2
        }

        #endregion OptionSets
    }

    /// <summary>DisplayName: Custom API Request Parameter, OwnershipType: UserOwned, IntroducedVersion: 1.0.0.0</summary>
    public static class Customapirequestparameter
    {
        public const string EntityName = "customapirequestparameter";
        public const string EntityCollectionName = "customapirequestparameters";

        #region Attributes

        /// <summary>Type: Uniqueidentifier, RequiredLevel: SystemRequired</summary>
        public const string PrimaryKey = "customapirequestparameterid";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string PrimaryName = "name";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string LogicalEntityName = "logicalentityname";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: systemuser</summary>
        public const string CreatedBy = "createdby";
        /// <summary>Type: Lookup, RequiredLevel: SystemRequired, Targets: customapi</summary>
        public const string CustomapiId = "customapiid";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string Description = "description";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string DisplayName = "displayname";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string EntityLogicalName = "entitylogicalname";
        /// <summary>Type: ManagedProperty, RequiredLevel: SystemRequired</summary>
        public const string Iscustomizable = "iscustomizable";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Ismanaged = "ismanaged";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Isoptional = "isoptional";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Custom API Field Type, OptionSetType: Picklist, DefaultFormValue: 0</summary>
        public const string Type = "type";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 128, Format: Text</summary>
        public const string UniqueName = "uniquename";

        #endregion Attributes

        #region Relationships

        /// <summary>Parent: "Customapi" Child: "Customapirequestparameter" Lookup: "CustomapiId"</summary>
        public const string RelM1_CustomapiCustomapirequestparameter = "customapi_customapirequestparameter";

        #endregion Relationships

        #region OptionSets

        public enum Type_OptionSet
        {
            Boolean = 0,
            DateTime = 1,
            Decimal = 2,
            Entity = 3,
            EntityCollection = 4,
            EntityReference = 5,
            Float = 6,
            Integer = 7,
            Money = 8,
            Picklist = 9,
            String = 10,
            StringArray = 11,
            GuId = 12
        }

        #endregion OptionSets
    }

    /// <summary>DisplayName: Custom API Response Property, OwnershipType: UserOwned, IntroducedVersion: 1.0.0.0</summary>
    public static class Customapiresponseproperty
    {
        public const string EntityName = "customapiresponseproperty";
        public const string EntityCollectionName = "customapiresponseproperties";

        #region Attributes

        /// <summary>Type: Uniqueidentifier, RequiredLevel: SystemRequired</summary>
        public const string PrimaryKey = "customapiresponsepropertyid";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string PrimaryName = "name";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string LogicalEntityName = "logicalentityname";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Component State, OptionSetType: Picklist</summary>
        public const string ComponentState = "componentstate";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: systemuser</summary>
        public const string CreatedBy = "createdby";
        /// <summary>Type: Lookup, RequiredLevel: SystemRequired, Targets: customapi</summary>
        public const string CustomapiId = "customapiid";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string Description = "description";
        /// <summary>Type: String, RequiredLevel: ApplicationRequired, MaxLength: 100, Format: Text</summary>
        public const string DisplayName = "displayname";
        /// <summary>Type: String, RequiredLevel: None, MaxLength: 100, Format: Text</summary>
        public const string EntityLogicalName = "entitylogicalname";
        /// <summary>Type: ManagedProperty, RequiredLevel: SystemRequired</summary>
        public const string Iscustomizable = "iscustomizable";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Ismanaged = "ismanaged";
        /// <summary>Type: Picklist, RequiredLevel: SystemRequired, DisplayName: Custom API Field Type, OptionSetType: Picklist, DefaultFormValue: 0</summary>
        public const string Type = "type";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 128, Format: Text</summary>
        public const string UniqueName = "uniquename";

        #endregion Attributes

        #region Relationships

        /// <summary>Parent: "Customapi" Child: "Customapiresponseproperty" Lookup: "CustomapiId"</summary>
        public const string RelM1_CustomapiCustomapiresponseproperty = "customapi_customapiresponseproperty";

        #endregion Relationships

        #region OptionSets

        public enum ComponentState_OptionSet
        {
            Published = 0,
            Unpublished = 1,
            Deleted = 2,
            DeletedUnpublished = 3
        }
        public enum Type_OptionSet
        {
            Boolean = 0,
            DateTime = 1,
            Decimal = 2,
            Entity = 3,
            EntityCollection = 4,
            EntityReference = 5,
            Float = 6,
            Integer = 7,
            Money = 8,
            Picklist = 9,
            String = 10,
            StringArray = 11,
            GuId = 12
        }

        #endregion OptionSets
    }

    /// <summary>DisplayName: Saved View, OwnershipType: UserOwned, IntroducedVersion: 5.0.0.0</summary>
    public static class UserQuery
    {
        public const string EntityName = "userquery";
        public const string EntityCollectionName = "userqueries";

        #region Attributes

        /// <summary>Type: Uniqueidentifier, RequiredLevel: SystemRequired</summary>
        public const string PrimaryKey = "userqueryid";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 200, Format: Text</summary>
        public const string PrimaryName = "name";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 1073741823</summary>
        public const string Columnsetxml = "columnsetxml";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 2000</summary>
        public const string Description = "description";
        /// <summary>Type: Memo, RequiredLevel: SystemRequired, MaxLength: 1073741823</summary>
        public const string Fetchxml = "fetchxml";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 1073741823</summary>
        public const string Layoutxml = "layoutxml";
        /// <summary>Type: Integer, RequiredLevel: SystemRequired, MinValue: 0, MaxValue: 1000000000</summary>
        public const string QueryType = "querytype";
        /// <summary>Type: EntityName, RequiredLevel: SystemRequired</summary>
        public const string ReturnedTypeCode = "returnedtypecode";
        /// <summary>Type: State, RequiredLevel: SystemRequired, DisplayName: Status, OptionSetType: State</summary>
        public const string StateCode = "statecode";
        /// <summary>Type: Status, RequiredLevel: None, DisplayName: Status Reason, OptionSetType: Status</summary>
        public const string StatusCode = "statuscode";

        #endregion Attributes

        #region OptionSets

        public enum ReturnedTypeCode_OptionSet
        {
        }
        public enum StateCode_OptionSet
        {
            Active = 0,
            Inactive = 1
        }
        public enum StatusCode_OptionSet
        {
            Active = 1,
            All = 3,
            Inactive = 2
        }

        #endregion OptionSets
    }

    /// <summary>DisplayName: Solution Component, OwnershipType: None, IntroducedVersion: 5.0.0.0</summary>
    public static class Solutioncomponent
    {
        public const string EntityName = "solutioncomponent";
        public const string EntityCollectionName = "solutioncomponentss";

        #region Attributes

        /// <summary>Type: Boolean, RequiredLevel: None, True: 1, False: 0, DefaultValue: True</summary>
        public const string Ismetadata = "ismetadata";
        /// <summary>Type: Uniqueidentifier, RequiredLevel: None</summary>
        public const string ObjectId = "objectid";
        /// <summary>Type: Lookup, RequiredLevel: None, Targets: solution</summary>
        public const string SolutionId = "solutionid";

        #endregion Attributes
    }

    /// <summary>DisplayName: View, OwnershipType: OrganizationOwned, IntroducedVersion: 5.0.0.0</summary>
    public static class Savedquery
    {
        public const string EntityName = "savedquery";
        public const string EntityCollectionName = "savedqueries";

        #region Attributes

        /// <summary>Type: Uniqueidentifier, RequiredLevel: SystemRequired</summary>
        public const string PrimaryKey = "savedqueryid";
        /// <summary>Type: String, RequiredLevel: SystemRequired, MaxLength: 200, Format: Text</summary>
        public const string PrimaryName = "name";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 1073741823</summary>
        public const string Columnsetxml = "columnsetxml";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Isdefault = "isdefault";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 2000</summary>
        public const string Description = "description";
        /// <summary>Type: EntityName, RequiredLevel: SystemRequired</summary>
        public const string ReturnedTypeCode = "returnedtypecode";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 1073741823</summary>
        public const string Fetchxml = "fetchxml";
        /// <summary>Type: Memo, RequiredLevel: None, MaxLength: 1073741823</summary>
        public const string Layoutxml = "layoutxml";
        /// <summary>Type: Integer, RequiredLevel: SystemRequired, MinValue: 0, MaxValue: 1000000000</summary>
        public const string QueryType = "querytype";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Isquickfindquery = "isquickfindquery";
        /// <summary>Type: Boolean, RequiredLevel: SystemRequired, True: 1, False: 0, DefaultValue: False</summary>
        public const string Ismanaged = "ismanaged";
        /// <summary>Type: State, RequiredLevel: SystemRequired, DisplayName: Status, OptionSetType: State</summary>
        public const string StateCode = "statecode";
        /// <summary>Type: Status, RequiredLevel: None, DisplayName: Status Reason, OptionSetType: Status</summary>
        public const string StatusCode = "statuscode";

        #endregion Attributes

        #region OptionSets

        public enum ReturnedTypeCode_OptionSet
        {
        }
        public enum StateCode_OptionSet
        {
            Active = 0,
            Inactive = 1
        }
        public enum StatusCode_OptionSet
        {
            Active = 1,
            Inactive = 2
        }

        #endregion OptionSets
    }
}

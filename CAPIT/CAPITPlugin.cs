using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using Rappen.XTB.CAPIT;

namespace Rappen.XTB.CAT
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Custom API Tester"),
        ExportMetadata("Description", "Browse Custom APIs, enter input parameters, execute the action, investigate output parameters."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAFXRFWHRDcmVhdGlvbiBUaW1lAAfkDAQIAQzALEbAAAAAB3RJTUUH5AwHFisxJiMEHQAAAAlwSFlzAAAK8AAACvABQqw0mAAAARRQTFRF///G3ufGrca9jKW9e5y9hKW9pb291t7GIVq1AEKtAEq9GFKta5S17/fGAEK1AFLWAFrvAGP/WoS1vc69AFLeAFrnEEqtlK29AErOAGP3EGvvY6WcKXvWlLW9//8Aa6WUc5y9IXPeY5ycnLW9WpylCEqtCGvvUpStSnu15+/GMYTOe62EGHPnCGv3xta9EHPntc5K9/cIhLV77/cQc62MKWO1CGP3zt4xrc5KjL1rvdZCjLVzY6WU5+8Yc6WM9//GQoy9IXverc5SSpS13uchlL1r1ucpjK29hLVzAErGOYTG9/8ApcZaxt45Upyl3u8YtdZKSpSt7/cItdZCnMZjUoS1CGPnY4y1QnO1Soy1OYy9c5S9B2wWswAAAAF0Uk5TAEDm2GYAAAIcSURBVHjabVN7X9pAEAyUhxcg4SCgRFyQEqkElBgqFRoeyiOh0FbFRtvv/z26uUtC+On8cUluht1Zbk4Q9phcndVd999xcSJ8hLRbJpLoOI5EtMLVOzr2QkSlQunbI6UVxSGv6UM+VRZLFNEx4Nx75kVtFeWTRKYMumXq/E0h7iTCK3yXzke7uf9aIm7Ax0P+Dap/YR0q/C6XZb8+pRur3TZ3wZeicaeuFOyoBiDMbvDtvLIBiee/ukb3p7DudKbQVG+5/g8poqCOBWoW/nLe3PS8/e3OAMO0TICF84KCI5neYd3lE0CjZdsjez0bbJ+fYPA87Je0HHbItwDGFdq9gceFsVkMoYVlBtDAtYI94oRuYcF6Tqn9Cx+/56GASithJV0DVH3buifQARdehzpZISGuwQjm0i1cesuIoMAEZij4cb/Zws8DwUqaAXR8gW3U+mPWbwBNvwWatGDM6Au6CJstQ5OXJK+D2faGsKat0ez+gR8r1NiYGK2v8u0AjG93Q4AR/uXwnQl4VaWMmUhK9LrnHZJx40WKl6Ym9HEVC95pa5iGdk1vMKcqd09VteslgoU3K1VoiAtfwCB+YXnIZU72eyofj0HWYjxSRRJGip77HljkPgehPAtDiVPUQr6+j/0xOfF9qKf+wckkEb0YxYyk0AhKYjl1eLVyWSLJec7mZZEUYu9vZ+KIEBFBSCabFj5ELP4pkUimDtj/b6JbJI4CPN0AAAAASUVORK5CYII="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#FFFFC0"),
        ExportMetadata("PrimaryFontColor", "#0000C0"),
        ExportMetadata("SecondaryFontColor", "#0000FF")]
    public class CAPITPlugin : PluginBase, IPayPalPlugin
    {
        public string DonationDescription => $"Donate to the creator of Custom Action Tester for XrmToolBox";

        public string EmailAccount => "jonas@rappen.net";

        public override IXrmToolBoxPluginControl GetControl()
        {
            return new CustomActionTester(new CAPITTool());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CAPITPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}
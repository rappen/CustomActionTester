using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.CAT
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Custom API Tester"),
        ExportMetadata("Description", "Browse Custom APIs, enter input parameters, execute the action, investigate output parameters."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAFXRFWHRDcmVhdGlvbiBUaW1lAAfkCxAVMSRTevQyAAAAB3RJTUUH5AsQFTgivtoJPgAAAAlwSFlzAAAK8AAACvABQqw0mAAAAPZQTFRF///G3ufGrca9jKW9e5y9hKW9pb291t7GIVq1AEKtAEq9GFKta5S17/fGAEK1AFLWAFrvAGP/WoS1vc69AFLeAFrnEEqtlK29AErOAGP3lLW9c5y9nLW9CEqtOYy9hLV7nMZjQoy9MYTGSoy1Snu15+/GEGvvtdZK//8AvdZCGHPnpcZaxta9xt459/cIc62MlL1r7/cQtdZCKWO1Y6WcOYTGUpSttc5K9//GjLVzrc5S9/8Ae62EjK29SpS1c62EAErGIXPeKXvW5+8YWpyljL1rxt4xY6WU1ucpzt4xrc5K3u8hMYTOUoS1Y4y1QnO1KXvOc5S9dp3VHwAAAAF0Uk5TAEDm2GYAAAHlSURBVHjabVN5X9pAFAyUww0h8DiikUYQK50EiWCL3DZiS1FqbL//l+nLHY75J9k383tvdndWkhJM7y5brvvvvD6VTqHoakKVHcdRhVG7O6Iz70Ju6ORDbzjio7jPFzS5SSlUZWOV5vOiQgdoCHea4htRfTa7Cf+awo34bMJ3gQ3FinDKrZb07wD4G08xAqeuGvM9255jFi+dD3+DIvF/j/4YL3q8F1FnQSts0OP6Dte0xWPS4p0FZ56DP2827EEbO6IlRhtzMp+bG5OaRpknVImebGDB/rz5z/xZsNy2d6TzjKzg2giTNVlDYPT6ugWWNMMbHzmRupJWbGGIjWfsN0JsqY15YEKRcjLRC356qwF+WYyujfES/UBQ8wQ94JkXa2DtVx/Q+YFOJOARFmDx4tEMivQ06ccCxTOpLzA+uMtoBJu85W0OUscbIDSpC47Wtwp9h+21sAbxGQ4x8a9L40zkVdIHsDv3wxHM5FYfvI9c827baFDvq79/04oEfV/bFH54FZVP6ardaV8nHro3V16DL34eyqULOomKkQkiVT+ObBC5z1EoL5NQpvlWEvtzcaEf9he59MOol9S9Jk1ZK+w/rbIi1Eo1jGJFFrXM8evMnQkhM4QoKUXpJDLZT7lcvrDH/gfvVVNhYVbBYAAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#FFFFC0"),
        ExportMetadata("PrimaryFontColor", "#0000C0"),
        ExportMetadata("SecondaryFontColor", "#0000FF")]
    public class CAPITPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new CustomActionTester(Tool.CAPIT);
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
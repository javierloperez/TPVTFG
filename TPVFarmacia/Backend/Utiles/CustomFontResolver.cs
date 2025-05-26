using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;

namespace TPVFarmacia.Backend.Utiles
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = "TPVFarmacia.Fuentes.arial.ttf"; 


            using Stream stream = assembly.GetManifestResourceStream(resource);
            if (stream == null)
                throw new InvalidOperationException($"No se encontró la fuente embebida: {resource}");

            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.ToLower().Contains("arial"))
            {
                return new FontResolverInfo("Arial#");
            }

            return null;
        }

        public string DefaultFontName => "Arial#";
    }
}

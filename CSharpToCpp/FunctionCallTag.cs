using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using System.IO;

namespace CSharpToCpp
{
    public class FunctionCallTag : DotLiquid.Tag
    {
        List<String> _Parts;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            _Parts = markup.Split('.').ToList();

            for (int x = 0; x < _Parts.Count(); x++)
                _Parts[x] = _Parts[x].Trim();

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            DotLiquid.Hash hash = context[_Parts[0]] as DotLiquid.Hash;

            if (hash == null)
                return;
        }
    }

    public class FunctionReturnTag : DotLiquid.Tag
    {
        List<String> _Parts;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            _Parts = markup.Split('.').ToList();

            for (int x = 0; x < _Parts.Count(); x++)
                _Parts[x] = _Parts[x].Trim();

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            DotLiquid.Hash hash = context[_Parts[0]] as DotLiquid.Hash;

            if (hash == null)
                return;
        }
    }
}

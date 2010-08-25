using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class BoolArray : ColladaArray<bool>
    {

        protected override bool Parse(string s)
        {
            return bool.Parse(s);
        }
    }
}
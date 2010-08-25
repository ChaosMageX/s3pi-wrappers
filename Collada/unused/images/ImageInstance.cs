using System;

namespace DAESim.DAE
{
    [Serializable]
    //[XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class ImageInstance : Instance<Image>
    {
        public ImageInstance()
        {
        }

        public ImageInstance(Image src) : base(src)
        {
        }
    }
}
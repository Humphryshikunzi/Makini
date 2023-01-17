using System.ComponentModel;

namespace _.Application.Enums
{
    public enum UploadType : byte
    {
        [Description(@"Images/Products")]
        Product,

        [Description(@"Images/ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document
    }
}
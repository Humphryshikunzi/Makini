using _.Application.Specifications.Base;
using _.Domain.Entities.Misc;

namespace _.Application.Specifications.Misc
{
    public class DocumentTypeFilterSpecification : AppSpecification<DocumentType>
    {
        public DocumentTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
// 1. Add NuGet packages, at a minimum you need Sitecore.XA.Foundation.Scriban with dependencies
using Scriban.Runtime;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;

namespace TheCodeAttic.Sitecore.ScribanGeneralLinkExt
{

    //2. Inherit from IGenerateScribanContextProcessor
    public class ScribanGeneralLink : IGenerateScribanContextProcessor
    {
        // 3. Implement the Process method as defined by the IGenerateScribanContextProcessor interface
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            // 3a. Import the fucntion name and its asscoiated logic
            args.GlobalScriptObject.Import(
                "sc_general_link", // name of the function that will be used within Scriban, to trigger the logic
                 new DoGeneralLink(GeneralLink)); // parameter two is a Delegate tied to the custom logic, method parameters are populated in order from the declaration within Scriban
        }

        // 4. Delegate declaration
        private delegate GeneralLinkDetails DoGeneralLink(Item item, string linkFieldName);

        // 5. Method which contains the actual logic that is to be performed, and called by the delegate
        /// <summary>
        /// Retrieves the named field from the item.
        /// IF the field is a LinkField THEN it is parsed into its parts for easy usage
        /// </summary>
        /// <param name="item">Sitecore Item</param>
        /// <param name="linkFieldName">Name of the Field to be Retrieved</param>
        /// <returns>NULL - if item or field do not exist. GeneralLinkDetails object, that contains the parsed link attributes</returns>
        public GeneralLinkDetails GeneralLink(Item item, string linkFieldName)
        {
            if (item == null)
                return null;

            if (item.Fields[linkFieldName] == null)
                return null;

            LinkField lnkField = (LinkField)item.Fields[linkFieldName];
            if (lnkField == null)
                return null;

            return new GeneralLinkDetails(lnkField);
        }
    }

    /// <summary>
    /// Model that contains fields representing each part of the General Link Field
    /// </summary>
    public class GeneralLinkDetails
    {
        public string linkurl { get; set; }
        public string linktarget { get; set; }
        public string linkcssclass { get; set; }
        public string linkdescription { get; set; }

        public GeneralLinkDetails() { }

        public GeneralLinkDetails(LinkField linkField)
        {
            linkurl = linkField.GetFriendlyUrl();
            linktarget = linkField.Target;
            linkcssclass = linkField.Class;
            linkdescription = linkField.Text;
        }
    }
}

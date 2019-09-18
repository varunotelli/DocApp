using DocApp.Data;
using DocApp.Domain.Callbacks;
using DocApp.Domain.DataContracts;
using DocApp.Models;
using DocApp.Presentation.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocApp.Domain.UseCase
{
    public class GetKeyWordUseCase : UseCaseBase, IKeyWordCallback
    {
        public List<KeyWord> keywords;
        IKeywordViewCallback keyWordCallback;
        
        public GetKeyWordUseCase()
        {
            keywords = new List<KeyWord>();
        }

        internal override async Task Action()
        {
            IKeywordList keyWordList = new KeywordDAO();


            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await keyWordList.GetKeywordsAsync(this);
                //System.Diagnostics.Debug.WriteLine("hosp val="+hospital.Number_Of_Rating);
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("Get keyword use case DB EXCEPTION" + e.Message);
            }


            if (keywords != null)
            {
                keyWordCallback.KeywordReadViewSuccess(keywords);
                //System.Diagnostics.Debug.WriteLine(hospital.Location);
            }
            else keyWordCallback.KeywordReadViewFail();
            // + hospitals.Count());

        }

        public bool KeyWordReadFail()
        {
            System.Diagnostics.Debug.WriteLine("Get Keyword use case fail");
            return false;
        }

        public bool KeyWordReadSuccess(List<KeyWord> k)
        {
            this.keywords = k;
            return true;
        }

        public override void SetCallBack<P>(P p)
        {
            this.keyWordCallback = (IKeywordViewCallback)p;
        }
    }
}

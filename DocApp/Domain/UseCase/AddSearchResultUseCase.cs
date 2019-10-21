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
    public class AddSearchResultUseCase : UseCaseBase, IDoc_SearchCallback
    {
        IDoc_SearchInsertViewCallback viewCallback;
        Doc_Search doc;
        int temp = -1;

        public AddSearchResultUseCase(Doc_Search d)
        {
            this.doc = d;
        }
        
        public override void SetCallBack<P>(P p)
        {
            this.viewCallback = (IDoc_SearchInsertViewCallback)p;
        }

        internal override async Task Action()
        {
            IDoc_Search doc_Search = new DocSearchDAO();
            try
            {
                await doc_Search.AddDocSearch(doc, this);
                if (temp > 0)
                    viewCallback.Doc_SearchInsertViewSuccess(temp);
                else viewCallback.Doc_SearchInsertViewFail();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("insert doc search fail "+e.Message);
            }
        }
        public bool Doc_SearchInsertFail()
        {
            return false;
        }

        public bool Doc_SearchInsertSuccess(int x)
        {
            this.temp = x;
            return true;
        }
    }
}

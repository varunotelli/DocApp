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
    public class UpdateHospitalRatingUseCase : UseCaseBase,IHospitalUpdateCallback
    {
        Hospital hospital;
        int id;
        double rating;

        IHospitalRatingUpdateViewCallback viewCallBack;

        public UpdateHospitalRatingUseCase(int x, double r)
        {
            this.id = x;
            this.rating = r;
        }

        public override void SetCallBack<P>(P p)
        {
            this.viewCallBack = (IHospitalRatingUpdateViewCallback)p;
        }

        internal override async Task Action()
        {
            IHospitalList hospitalList = new HospitalListDAO();
            await hospitalList.UpdateHospitalRating(id, rating, this);
            if (hospital != null)
                viewCallBack.HospitalUpdateSuccess(hospital);
            else
                viewCallBack.HospitalUpdateFail();
        }

        public bool HospitalUpdateFail()
        {
            return false;
        }

        public bool HospitalUpdateSuccess(Hospital h)
        {
            this.hospital = h;
            return true;
        }
    }
}

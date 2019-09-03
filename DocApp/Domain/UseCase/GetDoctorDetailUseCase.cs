﻿using DocApp.Data;
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
    class GetDoctorDetailUseCase : UseCaseBase, IDoctorCallback
    {
        Doctor doc = new Doctor();
        DoctorDetailViewCallBack useCaseCallback;
        public string name1;
        public GetDoctorDetailUseCase(string name)
        {
            this.name1 = name;
        }
        internal override async Task Action()
        {
            // = new IHospitalCallback();
            //hospitals = new List<Hospital>();
            IDoctorList DoctorList = new DoctorListDAO();

            try
            {
                System.Diagnostics.Debug.WriteLine("In use case");
                await DoctorList.GetDoctorByNameAsync(name1, this);
                
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                System.Diagnostics.Debug.WriteLine("DB EXCEPTION" + e.Message);
            }


            if (doc != null)
            {
                useCaseCallback.DataReadSuccess(doc);
            }
            else useCaseCallback.DataReadFail();
            // + hospitals.Count());


        }

        public override void SetCallBack<P>(P p)
        {
            this.useCaseCallback = (DoctorDetailViewCallBack)p;
        }


        public bool ReadSuccess(List<Doctor> docs)
        {

            this.doc = docs[0];
            System.Diagnostics.Debug.WriteLine("DOC DETAIL DAO READ SUCCESS!!!");

            return true;
        }
        public bool ReadFail()
        {
            System.Diagnostics.Debug.WriteLine("DOC DETAIL DAO READ FAIL!!!");
            return false;

        }
    }
}


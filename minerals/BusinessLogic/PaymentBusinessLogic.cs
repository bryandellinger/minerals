using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Minerals.BusinessLogic
{
    public class PaymentBusinessLogic : IPaymentBusinessLogic
    {
        private readonly IRoyaltyRepository royaltyRepository;
        private readonly IGenericRepository<Royalty> genericRoyaltyRepository;
        private readonly IGenericRepository<RoyaltyAdjustment> genericRoyaltyAdjustmentRepository;

        public PaymentBusinessLogic(IRoyaltyRepository royaltyRepo, IGenericRepository<Royalty> genericRoyaltyRepo, IGenericRepository<RoyaltyAdjustment> genericRoyaltyAdjustmentRepo)
        {
            royaltyRepository = royaltyRepo;
            genericRoyaltyRepository = genericRoyaltyRepo;
            genericRoyaltyAdjustmentRepository = genericRoyaltyAdjustmentRepo;
        }
        public object Save(RoyaltyViewModel model, long id)
        {
            if (model.SaveNewPaymentAsAdjustmentInd && model.Id < 1)
            {
                model.Id = royaltyRepository.CheckProdMonth(model.LesseeId, model.PostMonth.Value, model.PostYear.Value, model.PaymentTypeId, model.WellId).Id;
                model.AdjustmentInd = true;
                model.AdjustmentEntryDate = DateTime.Now;
            }
            if (model.Id > 0)
            {
                if (model.AdjustmentInd)
                {
                    Royalty royalty = genericRoyaltyRepository.GetById(model.Id);
                    genericRoyaltyAdjustmentRepository.Insert(new RoyaltyAdjustment
                    {
                        Id = 0,
                        EntryDate = model.AdjustmentEntryDate,
                        RoyaltyId = model.Id,
                        CheckId = model.CheckId,
                        Flaring = royalty.Flaring,
                        NRI = royalty.NRI,
                        GasProd = royalty.GasProd,
                        OilProd = royalty.OilProd,
                        GasRoyalty = royalty.GasRoyalty,
                        OilRoyalty = royalty.OilRoyalty,
                        SalesPrice = royalty.SalesPrice,
                        Deduction = royalty.Deduction,
                        LiqVolume = royalty.LiqVolume,
                        LiqPayment = royalty.LiqPayment,
                        TransDeduction = royalty.TransDeduction,
                        CompressDeduction = royalty.CompressDeduction,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        CreatedBy = id,
                        UpdatedBy = id,
                    });
                }
                royaltyRepository.Update(model, id);   
                return new Royalty { Id = model.Id };
            }
            else
            {
                Royalty royalty = genericRoyaltyRepository.Insert(new Royalty
                {
                 Id = 0,
                WellTractInformationId = model.WellTractInformationId,
                CheckId = model.CheckId,
                CheckNum = model.CheckNum,
                PaymentTypeId = model.PaymentTypeId,
                EntryDate = model.EntryDate,
                PostMonth = model.PostMonth,
                PostYear = model.PostYear,
                NRI = model.NRI,
                GasProd = model.GasProd,
                OilProd = model.OilProd,
                GasRoyalty = model.GasRoyalty,
                OilRoyalty = model.OilRoyalty,
                SalesPrice = model.SalesPrice,
                Deduction = model.Deduction,
                LiqVolume = model.LiqVolume,
                LiqPayment = model.LiqPayment,
                LiqMeasurement = model.LiqMeasurement,
                ProductTypeId = model.ProductTypeId,
                TransDeduction = model.TransDeduction,
                CompressDeduction = model.CompressDeduction,
                Flaring = model.Flaring,
                RoyaltyNotes = model.RoyaltyNotes,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                CreatedBy = id,
                UpdatedBy = id,
                });
                return new Royalty { Id = royalty.Id };
            }
           
        }
    }
}

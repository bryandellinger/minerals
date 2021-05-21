import * as constants from '../../constants';

export default function getShowLiqVolumeInd(
  paymentTypes, payment,
) {
  const allowedPaymentTypeNames = [
    constants.PaymentTypeLiquids,
  ];
  const allowedPaymentTypeIds = paymentTypes.filter(
    (x) => allowedPaymentTypeNames.includes(
      x.paymentTypeName,
    ),
  ).map((y) => y.id);
  return payment.paymentTypeId && allowedPaymentTypeIds.includes(
    payment.paymentTypeId,
  );
}

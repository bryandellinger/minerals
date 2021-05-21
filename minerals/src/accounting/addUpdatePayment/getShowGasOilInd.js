import * as constants from '../../constants';

export default function getShowGasOilInd(
  paymentTypes, payment,
) {
  const allowedPaymentTypeNames = [
    constants.PaymentTypeOilAndGas,
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

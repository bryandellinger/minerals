import numberWithCommas from './numberWithCommas';

export default function formatReadOnlyNumberInputs(container) {
  const inputs = $(`#${container} :input[type='number']:disabled`).toArray();
  inputs.forEach((input) => {
    const step = $(input).attr('step');
    if (step && parseFloat(step) < 1) {
      const value = parseFloat($(input).val());
      const tofixed = step.length - 1;
      $(input).attr('type', 'text');
      $(input).val(numberWithCommas((value || 0).toFixed(tofixed)));
    }
  });
}

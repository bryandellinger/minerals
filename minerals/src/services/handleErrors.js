export default function handleErrors(xhr, reject) {
  let error = '';
  switch (xhr.status) {
    case 403: // not authorized
      window.Toastr.error('not authorized');
      reject('not authorized');
      break;
    case 401: // not authorized
      window.Toastr.error('not authorized');
      reject('not authorized');
      break;
    default:
      console.log(xhr);
      error = `an error has occured: ${xhr.statusText}`;
      if (xhr.responseJSON && xhr.responseJSON.message) {
        error = xhr.responseJSON.message;
      }
      if (xhr.responseJSON && xhr.responseJSON.ValidationError) {
        error = xhr.responseJSON.ValidationError.toString();
      }
      window.Toastr.error(error, 'An error occured', { closeButton: true, timeOut: 20000 });
      reject(xhr.statusText);
      break;
  }
}

import handleErrors from './handleErrors';

export default class AjaxService {
  static ajaxGet(url) {
    return new Promise((resolve, reject) => {
      $.ajax({
        url,
        cache: false,
      })
        .done((responseData) => {
          resolve(responseData);
        })
        .fail((jqXHR) => {
          handleErrors(jqXHR, reject);
        });
    });
    }

    static ajaxDelete(url) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url,
                cache: false,
                type: 'DELETE'
            })
                .done((responseData) => {
                    resolve(responseData);
                })
                .fail((jqXHR) => {
                    handleErrors(jqXHR, reject);
                });
        });
    }


  static ajaxPut(url, data) {
    return new Promise((resolve, reject) => {
      $.ajax({
        url,
        cache: false,
        type: 'PUT',
        dataType: 'text json',
        contentType: 'application/json',
        data: JSON.stringify(data),
      })
        .done((responseData) => {
          resolve(responseData);
        })
        .fail((jqXHR) => {
          handleErrors(jqXHR, reject);
        });
    });
  }

  static ajaxPost(url, data) {
    return new Promise((resolve, reject) => {
      $.ajax({
        url,
        cache: false,
        type: 'POST',
        dataType: 'text json',
        contentType: 'application/json',
        data: JSON.stringify(data),
      })
        .done((responseData) => {
          resolve(responseData);
        })
        .fail((jqXHR) => {
          handleErrors(jqXHR, reject);
        });
    });
  }
}

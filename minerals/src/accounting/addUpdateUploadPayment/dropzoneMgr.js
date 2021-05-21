/* eslint-disable no-plusplus */
/* eslint-disable no-void */
/* eslint-disable no-underscore-dangle */
/* eslint-disable no-cond-assign */
/* eslint-disable no-return-assign */
/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
export default class DropzoneMgr {
  constructor(addUpdateUploadMgr) {
    this.addUpdateUploadMgr = addUpdateUploadMgr;
  }

  addTitle(file) {
    let preview = document.getElementsByClassName('dz-preview');
    preview = preview[preview.length - 1];
    const imageName = document.createElement('small');
    imageName.innerHTML = file.name;
    preview.insertBefore(imageName, preview.firstChild);
  }

  init() {
    const self = this;
    const { editmode } = self.addUpdateUploadMgr.state;

    Dropzone.options.uploader = {
      paramName: 'file',
    };

    $('#uploadPaymentDocuments').dropzone({
      url: './File/UploadFile',
      maxFiles: editmode ? 1 : 0,
      acceptedFiles: '.csv,text/csv,application/vnd.ms-excel,application/csv,text/x-csv,application/x-csv,text/comma-separated-values,text/x-comma-separated-values',
      init: function init() {
        const { files } = self.addUpdateUploadMgr.state;
        files.forEach((file) => {
          const mockFile = { name: file.fileName, size: file.fileSize };
          this.displayExistingFile(mockFile, `./images/${file.fileIcon}`);
          self.addTitle(mockFile);
        });
        this.on('thumbnail', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.addUpdateUploadMgr.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });

        // eslint-disable-next-line func-names
        this.on('maxfilesexceeded', function (file) {
          this.removeFile(file);
          window.Toastr.warning('please put unit in edit mode before uploading one file');
        });

        this.on('addedfile', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.addUpdateUploadMgr.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });
      },
      success: (file, response) => {
        const { files, uploadTemplates, uploadPayment } = self.addUpdateUploadMgr.state;
        files.push(response);
        let checkNoFromCSV = null;
        const fileNameParts = response.fileName.split('-');
        if (fileNameParts && fileNameParts.length === 5) {
          const year = parseInt(fileNameParts[0].trim(), 10);
          const month = parseInt(fileNameParts[1].trim(), 10);
          const day = parseInt(fileNameParts[2].trim(), 10);
          const checkDate = new Date(year, month - 1, day);
          const name = fileNameParts[3].trim();
          checkNoFromCSV = fileNameParts[4].replace(/\D/g, '');
          const uploadTemplate = uploadTemplates.find(
            (x) => x.templateName.toLowerCase() === name.toLowerCase(),
          );
          if (uploadTemplate) {
            self.addUpdateUploadMgr.setState({
              ...self.addUpdateUploadMgr.state,
              checkDate,
              files,
              uploadPayment: { ...uploadPayment, uploadTemplateId: uploadTemplate.id },
            });
          }
          self.addUpdateUploadMgr.render();
        } else {
          self.addUpdateUploadMgr.setState({ ...self.addUpdateUploadMgr.state, files });
          window.Toastr.error('File name must be in the form yyyy - mm - dd - template name - Check No. yourchecknum', 'File Name Error');
          return false;
        }
        if (checkNoFromCSV) {
          this.addUpdateUploadMgr.setState({ ...self.addUpdateUploadMgr.state, checkNoFromCSV });
          self.addUpdateUploadMgr.checkNumberMgr.getCheckFromCSV();
        }
      },
      addRemoveLinks: editmode,

      removedfile: (file) => {
        const { files } = self.addUpdateUploadMgr.state;
        for (let i = files.length - 1; i >= 0; i--) {
          if (files[i].fileName === file.name) {
            files.splice(i, 1);
          }
        }
        self.addUpdateUploadMgr.setState({ ...self.addUpdateUploadMgr.state, files });
        let _ref;
        return (_ref = file.previewElement) != null
          ? _ref.parentNode.removeChild(file.previewElement) : void 0;
      },

    });
  }
}

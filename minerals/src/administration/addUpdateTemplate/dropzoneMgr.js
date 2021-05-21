/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
export default class DropzoneMgr {
  constructor(addUpdateTemplateMgr) {
    this.addUpdateTemplateMgr = addUpdateTemplateMgr;
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
    const { editmode } = self.addUpdateTemplateMgr.state;

    Dropzone.options.uploader = {
      paramName: 'file',
    };

    $('#uploadDocuments').dropzone({
      url: './File/UploadFile',
      maxFiles: editmode ? 1 : 0,
      acceptedFiles: '.csv,text/csv,application/vnd.ms-excel,application/csv,text/x-csv,application/x-csv,text/comma-separated-values,text/x-comma-separated-values',
      init: function init() {
        const { files } = self.addUpdateTemplateMgr.state;
        files.forEach((file) => {
          const mockFile = { name: file.fileName, size: file.fileSize };
          this.displayExistingFile(mockFile, `./images/${file.fileIcon}`);
          self.addTitle(mockFile);
        });
        this.on('thumbnail', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.addUpdateTemplateMgr.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });

        this.on('maxfilesexceeded', function (file) {
          this.removeFile(file);
          window.Toastr.warning('please put unit in edit mode before uploading one file');
        });

        this.on('addedfile', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.addUpdateTemplateMgr.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });
      },
      success: (file, response) => {
        const { files } = self.addUpdateTemplateMgr.state;
        files.push(response);
        self.addUpdateTemplateMgr.setState({ ...self.addUpdateTemplateMgr.state, files });
      },
      addRemoveLinks: editmode,

      removedfile: (file) => {
        const { files } = self.addUpdateTemplateMgr.state;
        for (let i = files.length - 1; i >= 0; i--) {
          if (files[i].fileName === file.name) {
            files.splice(i, 1);
          }
        }
        self.addUpdateTemplateMgr.setState({ ...self.addUpdateTemplateMgr.state, files });
        let _ref;
        return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
      },

    });
  }
}

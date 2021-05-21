/* eslint-disable class-methods-use-this */
/* eslint-disable no-undef */
export default class DropzoneMgr {
  constructor(addUpdateCheckMgr) {
    this.addUpdateCheckMgr = addUpdateCheckMgr;
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
    const { editmode } = self.addUpdateCheckMgr.state;

    Dropzone.options.uploader = {
      paramName: 'file',
    };

    $('#uploadDocuments').dropzone({
      url: './File/UploadFile',
      maxFiles: editmode ? 10 : 0,
      init: function init() {
        const { files } = self.addUpdateCheckMgr.state;
        files.forEach((file) => {
          const mockFile = { name: file.fileName, size: file.fileSize };
          this.displayExistingFile(mockFile, `./images/${file.fileIcon}`);
          self.addTitle(mockFile);
        });
        this.on('thumbnail', (f) => {
          f.previewElement.addEventListener('click', () => {
            const file = self.addUpdateCheckMgr.state.files.find((x) => x.fileName === f.name);
            window.location.href = `./File/DownloadFile/${file.id}`;
          });
        });
        this.on('maxfilesexceeded', function (file) {
          this.removeFile(file);
          window.Toastr.warning('please put unit in edit mode before uploading files');
        });
      },
      success: (file, response) => {
        const { files } = self.addUpdateCheckMgr.state;
        files.push(response);
        self.addUpdateCheckMgr.setState({ ...self.addUpdateCheckMgr.state, files });
      },
      addRemoveLinks: editmode,

      removedfile: (file) => {
        const { files } = self.addUpdateCheckMgr.state;
        for (let i = files.length - 1; i >= 0; i--) {
          if (files[i].fileName === file.name) {
            files.splice(i, 1);
          }
        }
        self.addUpdateCheckMgr.setState({ ...self.addUpdateCheckMgr.state, files });
        let _ref;
        return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
      },

    });
  }
}

import AjaxService from '../../services/ajaxService';
import elementsToBeValidated from './elementsToBeValidated';

export default class ValidateAndSubmitMgr {
  constructor(AddUpdateWellMgr) {
    this.addUpdateWellMgr = AddUpdateWellMgr;
  }

  initFormSubmit() {
    // eslint-disable-next-line consistent-return
    $('#addUpdateWellForm').submit(() => {
      const {
        well, wellTractInfos, contractEventDetailReasonsForChangeId,
        digitalLogs, digitalImageLogs, hardCopyLogs, contractId, unitId, paymentId,
        wellboreShares, suretyId,
      } = this.addUpdateWellMgr.state;
      if (!wellTractInfos.length) {
        window.Toastr.error('Please add at least one well owner');
        return false;
      }

      this.addUpdateWellMgr.setState({ ...this.addUpdateWellMgr.state, submitInd: true });
      let formHasError = false;

      wellTractInfos.forEach((element, i) => {
        if (!element.tractId) {
          formHasError = true;
          $('.tractIdError').eq(i).show();
          $($('.tractId').eq(i).data('select2').$container).addClass('is-invalid');
        }
        if (!$('.pad').eq(i).val() && i === 0) {
          formHasError = true;
          $('.padNameError').eq(i).show();
          $('.pad').eq(i).addClass('is-invalid');
        }
      });

      // eslint-disable-next-line no-restricted-syntax
      for (const item of elementsToBeValidated) {
        if (this.addUpdateWellMgr.doesElementHaveError(item.element)) {
          formHasError = true;
        }
      }
      if (formHasError) {
        window.Toastr.error('Please fix errors and resubmit');
        return false;
      }
      $('.wellSubmitbtn').hide();
      $('.wellSubmitBtnDisabled').show();

      const newWell = {
        ...well,
        apiNum: this.addUpdateWellMgr.apiNumMgr.getDerivedApiNum(),
        wellStatusId: parseInt(well.wellStatusId, 10),
        lesseeId: well.lesseeId ? parseInt(well.lesseeId, 10) : null,
        wellTypeId: well.wellTypeId ? parseInt(well.wellTypeId, 10) : null,
        townshipId: well.townshipId ? parseInt(well.townshipId, 10) : null,
        deepestFormationId: well.deepestFormationId ? parseInt(well.deepestFormationId, 10) : null,
        producingFormationId:
         well.producingFormationId ? parseInt(well.producingFormationId, 10) : null,
        lat: well.lat ? parseFloat(well.lat) : null,
        long: well.long ? parseFloat(well.long) : null,
        elevation: parseInt(well.elevation, 10),
        hDepth: parseInt(well.hDepth, 10),
        vDepth: parseInt(well.vDepth, 10),
        wellTractInfos,
        contractEventDetailReasonsForChangeId,
        digitalLogs,
        digitalImageLogs,
        hardCopyLogs,
        glElevation: well.glElevation ? parseInt(well.glElevation, 10) : null,
        logStartDepth: well.logStartDepth ? parseInt(well.logStartDepth, 10) : null,
        logEndDepth: well.logEndDepth ? parseInt(well.logEndDepth, 10) : null,
        acreageAttributableToWells: well.acreageAttributableToWells ? parseFloat(well.acreageAttributableToWells) : null,
        wellboreShares: well.wellboreLengthInd ? wellboreShares : [],
      };
      AjaxService.ajaxPost('./api/WellMgrApi', newWell)
        .then((d) => {
          window.Toastr.success('Your well has been saved', 'Save Successful', { positionClass: 'toast-top-center' });
          this.addUpdateWellMgr.manageWellsMgr.init();
          this.addUpdateWellMgr.init(d.wellId, contractId, unitId, paymentId, suretyId);
        })
          .catch((e) => {
            console.log(e)
          $('.wellSubmitbtn').show();
          $('.wellSubmitBtnDisabled').hide();
        });
    });
  }
}

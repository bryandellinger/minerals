const template = require('../../views/wells/addUpdateWell/testLog.handlebars');

export default class TestLogMgr {
    constructor(AddUpdateWellMgr) {
        this.addUpdateWellMgr = AddUpdateWellMgr;
        this.container = 'testLogContainer'
    }

    render() {
        const {
            editmode, wellLogTestTypes, well,
            digitalLogs, digitalImageLogs, hardCopyLogs
        } = this.addUpdateWellMgr.state;
        const handlebarData = {
            editmode,
            well,
            digitalLogs: wellLogTestTypes.map(
                (test) => (
                    {
                        ...test,
                        selected: digitalLogs.includes(test.id),
                    }
                ),
            ),
            digitalImageLogs: wellLogTestTypes.map(
                (test) => (
                    {
                        ...test,
                        selected: digitalImageLogs.includes(test.id),
                    }
                ),
            ),
            hardCopyLogs: wellLogTestTypes.map(
                (test) => (
                    {
                        ...test,
                        selected: hardCopyLogs.includes(test.id),
                    }
                ),
            ),
        }
        const t = template(handlebarData);
        $(`#${this.container}`).empty().append(t);

        $('.select2-selectall').select2({ width: '100%', allowClear: true, closeOnSelect: false, });
        this.initSelectAll();
        this.initMultiSelectChange();
    }

    initSelectAll() {
        $('.select2-selectall').on('select2:select', (e) => {
            const {
                wellLogTestTypes,
            } = this.addUpdateWellMgr.state;
            const data = e.params.data;
            const element = $(e.target).attr('id');
            if (parseInt(data.id,10) === 0) {
                this.addUpdateWellMgr.state[element] = wellLogTestTypes.map(x => x.id);
                this.render();
            }
        });
    }

    initMultiSelectChange() {
        $('.select2-selectall').on('change', (e) => {
            const element = $(e.target).attr('id');
            const ids = $(e.target).select2('data').map(x => (parseInt(x.id, 10)));
            this.addUpdateWellMgr.state[element] = ids;
        });
    }
}
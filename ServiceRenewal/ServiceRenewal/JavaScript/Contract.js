//Declare variables

var StartDate = 'activeon';
var EndDate = 'expireson';
var Duration = 'billingfrequencycode';

function SetContractEndDate() {
    var startDate = Xrm.Page.getAttribute(StartDate).getValue();
    var interval = Xrm.Page.getAttribute(Duration).getValue();
    var result = null;
    if (startDate != null && interval != null) {
        result = new Date(startDate);
        if (interval == 1)
        {
            result.setMonth(result.getMonth() + 1);
            result = result.adjustDate(-1) ;
        }

        if (interval == 5)
        {
            result.setMonth(result.getMonth() + 12);
            result = result.adjustDate(-1);
        }

        Xrm.Page.getAttribute(EndDate).setValue(result);
    }
    else {
        Xrm.Page.getAttribute(EndDate).setValue(null);
    }

}
if (!Date.prototype.adjustDate) {
    Date.prototype.adjustDate = function (days) {
        var date;

        days = days || 0;

        if (days === 0) {
            date = new Date(this.getTime());
        } else if (days > 0) {
            date = new Date(this.getTime());

            date.setDate(date.getDate() + days);
        } else {
            date = new Date(
                this.getFullYear(),
                this.getMonth(),
                this.getDate() - Math.abs(days),
                this.getHours(),
                this.getMinutes(),
                this.getSeconds(),
                this.getMilliseconds()
            );
        }

        this.setTime(date.getTime());

        return this;
    };
}
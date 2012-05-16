$(document).ready(function () {
    $('#inputMessage').attr("rows", 1);
    $('#inputButton').hide();
    $('#inputMessage').focus(function () {
        $(this).attr("rows", 5);
        $('#inputButton').show();
        if (this.value == this.defaultValue) {
            this.value = '';
        }
        if (this.value != this.defaultValue) {
            this.select();
        }
    });
    $('#inputMessage').blur(function () {
        if (this.value == this.defaultValue || this.value == '') {
            $(this).attr("rows", 1);
            $('#inputButton').hide();
        }
    });

    $('#inputButton').click(function () {
        $('#writeMessageForm').submit();
    });
});
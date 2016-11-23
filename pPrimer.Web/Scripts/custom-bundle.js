$(function() {

    var submitButton = $("#submitButton");
    var waiterDiv = $("#waiterDiv");
    var calculationSid = $("#calculationSid");
    var statusBox = $("#statusBox");

    var onSubmitAjaxMethods = function (e) {

        e.preventDefault();

        var $form = $(this);

        if (!$form.valid())
            return false;

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        }

        statusBox.empty();

        $.ajax(options)
            .done(function(data) {
                var response = data;

                if (response.result === true) {

                    submitButton.hide();
                    waiterDiv.show();
                    $('*', $form).attr('disabled', 'disabled').attr('readonly', 'readonly');
                    calculationSid.val(response.sid);

                    $("form[data-cb-ajax-status='true'").submit();
                } else {

                    // ToDo: handle error
                }
            });

        return false;
    };

    var onRequestAjaxStatus = function () {

        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        }

        $.ajax(options)
            .done(function (data) {
                var response = data;

                if (response.IsSuccess === true) {

                    var arrayLength = response.StatusList.length;
                    for (var i = 0; i < arrayLength; i++) {
                        statusBox.append(response.StatusList[i]);
                        statusBox.append('<br/>');
                    }

                    if (!response.IsCompleted) {
                        // Wait for completeon
                        setTimeout(() => $("form[data-cb-ajax-status='true'").submit(), 1000);
                    } else {
                        // Completed
                        submitButton.show();
                        waiterDiv.hide();
                        $('*', $("form[data-cb-ajax-methods='true'")).removeAttr('disabled').removeAttr('readonly');
                    }
                } else {

                    // ToDo: handle response.IsSuccess === "false"
                }
            });

        return false;
    };

    $("form[data-cb-ajax-methods='true'").submit(onSubmitAjaxMethods);
    $("form[data-cb-ajax-status='true'").submit(onRequestAjaxStatus);
})
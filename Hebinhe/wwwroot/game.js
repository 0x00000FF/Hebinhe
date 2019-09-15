(function ($) {
    $(function () {
        var current = null;
        var correct = 0;
        var incorrect = 0;
        var time = 0;

        function updateQuestion() {
            $.get("/api/game")
                .done(data => {
                    current = data;

                    var body = current.value
                        .toString(current.type)
                        .toUpperCase();
                    var html = body + "<sub>(" + current.type + ")</sub>";
                    var targetBase = current.type == 2 ? "Hexadecimal" : "Binary";

                    $("#question").html(html);

                    $("#answer").attr("placeholder", "To " + targetBase + "...")
                                .val('');
                });
        }

        $("#answer").keypress(
            event => {
                if (event.keyCode == 13) {
                    current.answer = $("#answer").val();

                    $.ajax({
                        url: "/api/game",
                        method: "POST",
                        data: JSON.stringify(current),
                        contentType: "application/json; utf-8",
                        success: data => {
                            if (data) {
                                correct++;
                            } else {
                                incorrect++;
                            }

                            $("#correct").text(correct);
                            $("#incorrect").text(incorrect);

                            updateQuestion();
                        }
                    });
                }
            });

        updateQuestion();

        setInterval(() => {
            time++;
            $("#time").text(Math.floor(time / 60) + ":" + (time % 60));
        }, 1000);
    });
})(jQuery);
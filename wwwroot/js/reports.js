function myFunction() {
    // Declare variables
    var input, filter, ul, li, a, i, txtValue;
    input = document.getElementById('myInput');
    filter = input.value.toUpperCase();
    ul = document.getElementById("reportList");
    li = ul.getElementsByTagName('li');

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        txtValue = a.textContent || a.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

$('.delete').on('click', function () {
    $(this).parent().remove();
});



function showMoveIns() {
    var showMoveIn = document.getElementById('unitMoveIns');
    showMoveIn.style.display = "block";
}

function showMoveOuts() {
    var showMoveOut = document.getElementById('unitMoveOuts');
    showMoveOut.style.display = "block";
}

function hideMoveIns() {
    var hideMoveIn = document.getElementById('unitMoveIns');
    hideMoveIn.style.display = "none";
}

function hideMoveOuts() {
    var hideMoveOut = document.getElementById('unitMoveOuts');
    hideMoveOut.style.display = "none";
}






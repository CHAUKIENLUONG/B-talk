/*// Global variable to store total money
var totalMoney = 0;
var totalTicketMoney = 0;
var selectedSeats = []; // Array to store selected seat IDs
var selectedCombos = []; // Array to store selected combo IDs

$(document).ready(function () {
    // Event listener for plus and minus buttons
    $('.increase').click(function () {
        var input = $(this).closest('.combo-number').find('input');
        var currentValue = parseInt(input.val());
        input.val(currentValue + 1);
        updateTable(); // Update combo table when button is clicked
    });

    $('.decrease').click(function () {
        var input = $(this).closest('.combo-number').find('input');
        var currentValue = parseInt(input.val());
        if (currentValue > 0) {
            input.val(currentValue - 1);
            updateTable(); // Update combo table when button is clicked
        }
    });

    // Function to handle seat selection
    $('.seat').click(function () {
        var seatId = $(this).data('seat-id');
        returnSeatValue(seatId); // Update seat selection
    });

    // Function to handle combo selection
    $('.combo').click(function () {
        selectedCombos = []; // Clear the selectedCombos array
        updateTable(); // Update combo table
    });
});

// Function to handle seat selection
function returnSeatValue(seatId) {
    var index = selectedSeats.indexOf(seatId); // Check if seat is already selected
    if (index === -1) {
        // Seat not selected, add to selectedSeats array
        selectedSeats.push(seatId);
        // Update seat image to 'seat_ischosen.png'
        $('[data-seat-id="' + seatId + '"] .seat-pic').attr('src', '/images/seat/seat_ischosen.png');
        totalTicketMoney += 65000;
        // Add seat price to total money
        totalMoney += 65000; // Assuming each seat costs 65,000 VND
    } else {
        // Seat already selected, remove from selectedSeats array
        selectedSeats.splice(index, 1);
        // Update seat image to 'seat_empty.png'
        $('[data-seat-id="' + seatId + '"] .seat-pic').attr('src', '/images/seat/seat_empty.png');
        totalTicketMoney -= 65000;
        // Subtract seat price from total money
        totalMoney -= 65000; // Assuming each seat costs 65,000 VND
    }

    updateTicketTable(); // Update the ticket table
}

// Function to update the ticket table with selected seats
function updateTicketTable() {
    var tableBody = $('#ticketTableBody');
    tableBody.empty(); // Clear existing rows

    // Iterate over selected seats and add them to the table
    selectedSeats.forEach(function (seatId) {
        var seatInfo = seatId.slice(4); // Extract seat information (e.g., 'A1')
        var seatRow = '<tr><td class="td-first" style="padding-right: 10px;">' + seatInfo + '</td><td class="td">x 65,000 VND</td></tr>';
        tableBody.append(seatRow);
    });

    // Update total money in combo table
    $('#comboTableBody .total-numb').text(totalMoney.toLocaleString());
}

// Function to update the table with selected combos
function updateTable() {
    var tableBody = $('#comboTableBody');
    tableBody.empty(); // Clear existing rows

    // Initialize total price of selected combos
    var totalPriceOfCombos = 0;

    // Iterate over each combo
    $('.combo').each(function () {
        var comboId = $(this).data('combo-id');
        var comboName = $(this).find('.combo-info h4').text();
        var quantity = parseInt($(this).find('.Qlty').val());
        var comboPrice = parseFloat($(this).find('.combo-price').text()); // Retrieve combo price from hidden input

        // Add a row for each combo with non-zero quantity
        if (quantity > 0) {
            var totalPrice = quantity * comboPrice;
            totalPriceOfCombos += totalPrice;

            // Create and append table row
            var comboRow = '<tr class="tr_first">' +
                '<td class="td-first"><span class="names">' + comboName + '</span></td>' +
                '<td></td>' + // Empty td element for space
                '<td class="td-second align-right"><span class="sol">' + quantity + '</span> x ' + comboPrice.toLocaleString() + ' VNĐ</td>'
            '</tr>';

            tableBody.append(comboRow);

            // Add the combo ID to the selectedCombos array based on its quantity
            for (var i = 0; i < quantity; i++) {
                selectedCombos.push(comboId); // Add combo ID to the array 'quantity' times
            }
        }
    });

    // Calculate the difference between the new total price and the old total price
    var priceDifference = totalPriceOfCombos - (totalMoney - totalTicketMoney);

    // Update totalMoney with the difference
    totalMoney += priceDifference;

    // Add total row
    var totalRow = '<tr class="tr_second align-right">' +
        '<td colspan="4"><span class="total-text-">Tổng giá tiền</span></td>' +
        '</tr>' +
        '<tr class="align-right">' +
        '<td colspan="4"><span class="total-numb">' + totalMoney.toLocaleString() + '</span><span style="font-size: 24px; font-weight: bold;">VNĐ</span></td>' +
        '</tr>';
    tableBody.append(totalRow);

    // Update hidden input fields with selected seat IDs and combo IDs
    $('#selectedSeats').val(selectedSeats.join(',')); // Join selected seat IDs into a comma-separated string
    $('#selectedCombos').val(selectedCombos.join(',')); // Join selected combo IDs into a comma-separated string
    $('#totalMoney').val(parseInt(totalMoney));

    // Enable/disable payment button based on selected seats
    var paymentButton = $('#paymentButton');
    if (selectedSeats.length === 0) {
        paymentButton.prop('disabled', true);
    } else {
        paymentButton.prop('disabled', false);
    }
}

// Initial table update
updateTable();
*/

$(document).ready(function () {
    var selectedSeats = []; // Mảng lưu các ghế được chọn
    var totalMoney = 0; // Tổng tiền thanh toán cho ghế
    var selectedCombos = []; // Mảng lưu các combo được chọn
    var totalComboMoney = 0; // Tổng tiền thanh toán cho combo

    // Xử lý sự kiện khi người dùng chọn ghế
    $('.seat').click(function () {
        var seatId = $(this).data('seat-id');
        var seatPrice = parseInt($(this).find('.seat-price').text());

        // Kiểm tra xem ghế đã được chọn chưa
        if (!$(this).hasClass('selected')) {
            // Thêm ghế vào mảng selectedSeats
            selectedSeats.push(seatId);
            totalMoney += seatPrice;
            $(this).addClass('selected');
        } else {
            // Nếu đã chọn rồi, hủy bỏ chọn
            var index = selectedSeats.indexOf(seatId);
            if (index > -1) {
                selectedSeats.splice(index, 1);
                totalMoney -= seatPrice;
            }
            $(this).removeClass('selected');
        }

        // Cập nhật giá trị cho trường hidden selectedSeats và totalMoney
        $('#selectedSeats').val(selectedSeats.join(',')); // Chuyển mảng thành chuỗi
        $('#totalMoney').val(totalMoney);
    });

    // Xử lý sự kiện khi người dùng ấn nút thanh toán cho ghế
    $('#paymentButton').click(function () {
        // Validate dữ liệu ở đây nếu cần thiết
        // Gửi form đi
        $('#paymentForm').submit();
    });

    // Xử lý sự kiện khi người dùng ấn nút thanh toán cho combo
    $('#comboPaymentButton').click(function () {
        selectedCombos = []; // Reset mảng các combo được chọn
        totalComboMoney = 0; // Reset tổng tiền thanh toán cho combo

        // Lặp qua các combo để lấy thông tin
        $('.combo').each(function () {
            var comboId = $(this).data('combo-id');
            var comboPrice = parseInt($(this).find('.combo-price').text());
            var quantity = parseInt($(this).find('.Qlty').val());

            // Kiểm tra xem combo có được chọn không
            if (quantity > 0) {
                var comboTotalPrice = comboPrice * quantity;
                selectedCombos.push({
                    comboId: comboId,
                    quantity: quantity
                });
                totalComboMoney += comboTotalPrice;
            }
        });

        // Cập nhật giá trị cho trường hidden selectedCombos và totalComboMoney
        $('#selectedCombos').val(JSON.stringify(selectedCombos)); // Chuyển mảng thành JSON string
        $('#totalComboMoney').val(totalComboMoney);

        // Validate dữ liệu ở đây nếu cần thiết
        // Gửi form đi
        $('#comboPaymentForm').submit();
    });
});
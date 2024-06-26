$(document).ready(function () {
    // Set the maximum date for the date input to today
    var today = new Date().toISOString().split('T')[0];
    $('#dob').attr('max', today);

    // Function to handle form submission
    function checkUserNameExists(username, callback) {
        $.ajax({
            url: '/api/users/username/' + username,
            type: 'GET',
            success: function (exists) {
                callback(exists);
            }
        });
    }

    // Form submission event handler
    $('#userForm').submit(function (event) {
        event.preventDefault(); // Prevent default form submission

        var username = $('#username').val();

        checkUserNameExists(username, function (exists) {
            if (exists) {
                alert('Username already exists.');
            } else {
                // Serialize form data into FormData object
                var formData = new FormData();
                formData.append('username', username);
                formData.append('email', $('#email').val());
                formData.append('dob', $('#dob').val());
                formData.append('photo', $('#photo')[0].files[0]);

                // Ajax request to upload photo and save user data
                $.ajax({
                    url: '/api/users',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        // On success, clear the form fields
                        clearForm();

                        // Refresh the users list
                        getUsersList();
                    }
                });
            }
        });
    });
    // Function to clear the form fields
    function clearForm() {
        $('#username').val('');
        $('#email').val('');
        $('#dob').val('');
        $('#photo').val('');
        $('#photo-preview').attr('src', '#').hide(); // Hide the preview
    }

    // Event handler for the clear button
    $('#clearBtn').click(function () {
        clearForm();
    });

    // Function to fetch and display users list
    function getUsersList() {
        $.ajax({
            url: '/api/users', // Endpoint to fetch users list (replace with your ASP.NET Core endpoint)
            type: 'GET',
            success: function (users) {
                // Clear existing table rows
                $('#usersTable tbody').empty();

                // Iterate over fetched users and append rows to the table
                users.forEach(function (user) {
                    var dateOfBirth = formatDate(new Date(user.dateOfBirth)); // Format the date
                    var row = '<tr data-id="' + user.id + '">' +
                        '<td>' + user.username + '</td>' +
                        '<td>' + user.email + '</td>' +
                        '<td>' + dateOfBirth + '</td>' +
                        '<td><img src="' + user.photo + '" style="max-width: 100px;"></td>' +
                        '</tr>';
                    $('#usersTable tbody').append(row);
                });

                // Attach click event to each row to display full user form
                $('#usersTable tbody tr').click(function () {
                    var userId = $(this).data('id');
                    getUserDetails(userId);
                });
            }
        });
    }

    // Function to fetch and display full user details
    function getUserDetails(userId) {
        $.ajax({
            url: '/api/users/' + userId, // Endpoint to fetch user details by ID (replace with your ASP.NET Core endpoint)
            type: 'GET',
            success: function (user) {
                // Populate form fields with fetched user details
                $('#username').val(user.username);
                $('#email').val(user.email);
                // Correctly handle date to avoid time zone issues
                var date = new Date(user.dateOfBirth);
                $('#dob').val(formatForInput(date)); // Format date for input[type="date"]
                $('#photo').val('');

                // Display the photo
                $('#photo-preview').attr('src', user.photo).show();
            }
        });
    }

    // Function to format date to dd/mm/yyyy
    function formatDate(date) {
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
        var year = date.getFullYear();
        return day + '/' + month + '/' + year;
    }

    // Function to format date to yyyy-mm-dd for input[type="date"]
    function formatForInput(date) {
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0'); // Months are zero-based
        var year = date.getFullYear();
        return year + '-' + month + '-' + day;
    }

    // Image preview function
    $("#photo").change(function () {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#photo-preview').attr('src', e.target.result).show();
        };
        reader.readAsDataURL(this.files[0]);
    });

    // Initial load of users list
    getUsersList();
});

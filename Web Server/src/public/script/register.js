const elements = {
    username: id('username'),
    password: id('password'),
    submit: id('submit'),
    message: id('message')
}

elements.submit.addEventListener('click', () => {
    const username = elements.username.value
    const password = elements.password.value

    axios.post('/api/register', {
        username,
        password
    }).then((response) => {
        const data = response.data

        if (data.status === StatusCode.REGISTER_ACCOUNT_ALREADY_EXISTS) {
            updateMessage('Account already exists')
            return
        }

        if (data.status === StatusCode.REGISTER_USERNAME_INVALID) {
            updateMessage('Invalid username')
            return
        }

        if (data.status === StatusCode.REGISTER_PASSWORD_INVALID) {
            updateMessage('Invalid password')
            return
        }

        if (data.status === StatusCode.REGISTER_SUCCESS) {
            updateMessage('Account registered!')
        }
    }).catch((error) => {
        console.log(error)
    })
})

function updateMessage(message) {
    elements.message.innerHTML = message
}
const elements = {
    username: id('username'),
    password: id('password'),
    submit: id('submit'),
    message: id('message')
}

elements.submit.addEventListener('click', () => {
    const username = elements.username.value
    const password = elements.password.value

    axios.post('/api/login', {
        username,
        password
    }).then((response) => {
        const data = response.data

        if (data.status === StatusCode.LOGIN_DOESNT_EXIST) {
            updateMessage('Login doesn\'t exist')
            return
        }

        if (data.status === StatusCode.LOGIN_WRONG_PASSWORD) {
            updateMessage('Wrong password')
            return
        }

        if (data.status === StatusCode.LOGIN_SUCCESS) {
            updateMessage('Login success')
            return
        }
    }).catch((error) => {
        console.log(error)
    })
})

function updateMessage(message) {
    elements.message.innerHTML = message
}
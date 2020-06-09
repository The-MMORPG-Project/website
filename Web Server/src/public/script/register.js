const elements = {
    email: id('email'),
    username: id('username'),
    password: id('password'),
    confirmPassword: id('confirmPassword'),
    submit: id('submit'),
    title: id('title'),
    subtitle: id('subtitle'),
    emailDiv: id('email-div'),
    message: id('message')
}

const formElements = [email, username, password, confirmPassword]


const resetClasses = () => {
    formElements.forEach(element => {
        if (element.classList.contains("invalid")) {
            element.classList.remove("invalid")
        }
    })
}

const validateForm = () => {
    removeMessage('notifyEmail')
    removeMessage('notifyUsername')
    removeMessage('notifyPassword')
    removeMessage('notifyPasswordConfirm')

    let emailValid = true
    let usernameValid = true
    let passwordValid = true
    let passwordConfirmValid = true

    // Validate email
    const email = elements.email
    if (!/^\S+@\S+$/.test(email.value) || email.value === "") {
        createMessage('notifyEmail', 'container-email', 'Please provide a valid email', 'The email is invalid')
        email.classList.add("invalid")

    } else if (email.value.length < 6 || email.value.length > 50) {
        createMessage('notifyEmail', 'container-email', 'Please provide a valid email', 'Must be between 6 and 50 characters')
        email.classList.add("invalid")
        emailValid = false
    }

    // Validate username
    const username = elements.username
    if (!/[a-zA-Z0-9_]/.test(username.value) || username.value === "") {
        createMessage('notifyUsername', 'container-username', 'Please provide a valid username', 'Must contain only alphanumeric characters')
        username.classList.add("invalid")
    } else if (username.value.length < 3 || username.value.length > 15) {
        createMessage('notifyUsername', 'container-username', 'Please provide a valid username', 'Must be between 3 to 15 characters')
        username.classList.add("invalid")
        usernameValid = false
    }

    // Validate password
    const password = elements.password
    if (password.value === "" || password.value.length < 5 || password.value.length > 30) {
        createMessage('notifyPassword', 'container-password', 'Please provide a valid password', 'Must be between 5 to 30 characters')
        password.classList.add("invalid")
        passwordValid = false
    }

    // Validate confirm password
    const confirmPassword = elements.confirmPassword
    if (password.value !== confirmPassword.value || confirmPassword.value === '') {
        createMessage('notifyPasswordConfirm', 'container-password-confirm', 'Please provide a valid password', 'Passwords do not match')
        confirmPassword.classList.add("invalid")
        passwordConfirmValid = false
    }

    if (!emailValid || !usernameValid || !passwordValid || !passwordConfirmValid) {
        return false
    }

    return true
}

const sendForm = () => {
    console.log("Sending register request to server...")

    axios.post('/api/register', {
        username: elements.username.value,
        password: elements.password.value
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
}

elements.submit.addEventListener('click', () => {
    resetClasses()
    setTimeout(() => {
        if (!validateForm()) {
            return
        }

        sendForm()
    }, 1)
})

function createMessage(id, appendID, titleText, subtitleText) {
    const div = document.createElement('div')
    const title = document.createElement('p')
    const subtitle = document.createElement('p')

    div.setAttribute('id', id)
    div.setAttribute('class', 'message')

    title.setAttribute('class', 'title')
    subtitle.setAttribute('class', 'subtitle')

    div.appendChild(title)
    div.appendChild(subtitle)

    title.innerText = titleText
    subtitle.innerHTML = subtitleText

    const container = document.getElementById(appendID)
    container.appendChild(div)
}

function removeMessage(id) {
    const element = document.getElementById(id)
    if (element != null) {
        element.parentNode.removeChild(element)
    }
}

function updateMessage(text) {
    const element = document.getElementById('serverMessage')
    element.style.visibility = 'visible'
    element.innerHTML = text
}
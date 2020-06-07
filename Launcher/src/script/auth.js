const { BrowserWindow, BrowserView } = require('electron').remote

const elements = {
    switcher: id('switcher'),
    btnClose: id('closeButton'),
    stackLog: id('stackLogin'),
    stackReg: id('stackRegister'),
    btnLog: id('loginButton'),
    btnReg: id('registerButton'),
}

elements.btnClose.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

elements.switcher.addEventListener('click', () => {
    /* Switch from login/register to register/login */
    switch (elements.switcher.innerHTML)
    {
        case 'Register':
            elements.stackLog.style = 'display: none;'
            elements.stackReg.style = ''
            elements.switcher.innerHTML = 'Login'
            break
        default:
            elements.stackReg.style = 'display: none;'
            elements.stackLog.style = ''
            elements.switcher.innerHTML = 'Register'
    }
})

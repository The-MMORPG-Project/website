const { BrowserWindow } = require('electron').remote
const close = document.getElementById('close')
const cancel = document.getElementById('cancel')
const apply = document.getElementById('apply')

close.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

cancel.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

apply.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

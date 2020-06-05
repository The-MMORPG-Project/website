const { BrowserWindow } = require('electron').remote

const elements = {
    close: id('close'),
    cancel: id('cancel'),
    apply: id('apply')
}

elements.close.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

elements.cancel.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

elements.apply.addEventListener('click', () => {
    BrowserWindow.getFocusedWindow().close()
})

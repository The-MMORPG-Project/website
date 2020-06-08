const config = {
    online: true,
    server: {
        port: 3000
    },
    username: {
        min_length: 3,
        max_length: 15
    },
    password: {
        min_length: 5,
        max_length: 30
    }
}

export { config }
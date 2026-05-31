import axios from "axios";

const api = axios.create({
    baseURL: "http://localhost:5234/api"
});

export default api;
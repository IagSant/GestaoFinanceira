import { Routes, Route } from "react-router-dom";

import LoginPage from "./pages/LoginPage";
import AdminPage from "./pages/AdminPage";
import UsuarioPage from "./pages/UsuarioPage";

function App() {
    return (
        <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/admin" element={<AdminPage />} />
            <Route path="/usuario" element={<UsuarioPage />} />
        </Routes>
    );
}

export default App;
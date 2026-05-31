import { useState } from "react";
import api from "../services/api";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
    const [email, setEmail] = useState("");
    const [senha, setSenha] = useState("");

    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await api.post("/Auth/login", {
                email,
                senha
            });

            localStorage.setItem("token", response.data.token);
            localStorage.setItem("role", response.data.role);
            localStorage.setItem("nome", response.data.nome);

            console.log("LOGIN OK");
            console.log(response.data);

            if (response.data.role === "ADMIN") {
                navigate("/admin");
            } else {
                navigate("/usuario");
            }

        } catch (error: any) {
            console.error("ERRO:", error);

            if (error.response) {
                console.log("STATUS:", error.response.status);
                console.log("DADOS:", error.response.data);
            }

            alert("Erro ao realizar login");
        }
    };

    return (
        <main>
            <h1>Login</h1>

            <form>
                <div>
                    <label>Email</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </div>

                <div>
                    <label>Senha</label>
                    <input
                        type="password"
                        value={senha}
                        onChange={(e) => setSenha(e.target.value)}
                    />
                </div>

                <button
                    type="button"
                    onClick={handleLogin}
                >
                    Entrar
                </button>
            </form>
        </main>
    );
}
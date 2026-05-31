import { useNavigate } from "react-router-dom";
import "./Sidebar.css";

export default function Sidebar() {

    const navigate = useNavigate();

    const logout = () => {

        localStorage.clear();

        navigate("/");
    };

    return (
        <aside>

            <h2>Admin</h2>

            <button
                onClick={() => navigate("/admin")}
            >
                Dashboard
            </button>

            <button
                onClick={() => navigate("/usuarios")}
            >
                Usuários
            </button>

            <button
                onClick={() => navigate("/contas")}
            >
                Contas
            </button>

            <button
                onClick={() => navigate("/transacoes")}
            >
                Transações
            </button>

            <button
                onClick={() => navigate("/relatorios")}
            >
                Relatórios
            </button>

            <button
                onClick={() => navigate("/usuario")}
            >
                Modo Usuário
            </button>

            <button
                onClick={logout}
            >
                Sair
            </button>

        </aside>
    );
}
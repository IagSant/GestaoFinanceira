import Sidebar from "../components/Sidebar";
import "../components/AdminPage.css";
export default function AdminPage() {

    const nome = localStorage.getItem("nome");

    return (
        <div className="admin-container">

            <Sidebar />

            <main className="admin-content">

                <h1>Painel Administrativo</h1>

                <p>
                    Bem-vindo, <strong>{nome}</strong>
                </p>

            </main>

        </div>
    );
}
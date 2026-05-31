import { useEffect, useState } from "react";
import api from "../services/api";

export default function UsuarioPage() {

    const [usuarios, setUsuarios] = useState<any[]>([]);

    useEffect(() => {
        carregarUsuarios();
    }, []);

    const carregarUsuarios = async () => {
        try {
            const response = await api.get("/Usuario");
            setUsuarios(response.data);
        } catch (error) {
            console.error(error);
        }
    };

    return (
        <main>
            <h1>Usuários</h1>

            <table border={1}>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Nome</th>
                        <th>Email</th>
                        <th>Role</th>
                    </tr>
                </thead>

                <tbody>
                    {usuarios.map((usuario: any) => (
                        <tr key={usuario.id}>
                            <td>{usuario.id}</td>
                            <td>{usuario.nome}</td>
                            <td>{usuario.email}</td>
                            <td>{usuario.role}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </main>
    );
}
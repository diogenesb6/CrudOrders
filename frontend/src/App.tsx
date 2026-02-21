import { Routes, Route, Link } from 'react-router-dom';
import PedidoList from './components/PedidoList';
import PedidoForm from './components/PedidoForm';
import './App.css';

export default function App() {
  return (
    <div className="app">
      <header className="app-header">
        <Link to="/" className="app-title">📦 CRUD Pedidos</Link>
      </header>
      <main className="app-main">
        <Routes>
          <Route path="/" element={<PedidoList />} />
          <Route path="/pedidos/novo" element={<PedidoForm />} />
          <Route path="/pedidos/:id" element={<PedidoForm />} />
        </Routes>
      </main>
    </div>
  );
}

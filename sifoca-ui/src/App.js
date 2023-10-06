import React from "react";
import { Route, BrowserRouter as Router, Routes } from "react-router-dom";
import { Entrada, Inicio, Movimento, Saida } from "./pages";
import SidebarComponent from "./templates/SidebarComponent";

function App() {
  return (
    <>
      <Router>
        <SidebarComponent>
          <Routes>
            <Route path="/" element={<Inicio />} exact />
            <Route path="/movimentos" element={<Movimento />} />
            <Route path="/entradas" element={<Entrada />} />
            <Route path="/saidas" element={<Saida />} />
          </Routes>
        </SidebarComponent>
      </Router>
    </>
  );
}

export default App;

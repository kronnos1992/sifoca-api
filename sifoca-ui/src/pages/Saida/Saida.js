import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { getAllSaidas } from "../../redux/actions/saidaActions";
import GetSaidasList from "./GetSaidaList";

const Saida = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(getAllSaidas);
  }, [dispatch]);

  return (
    <div style={{ maxWidth: "70%", margin: "auto" }}>
      <h4 className="text-center">RELATÓRIO DE DESPESAS</h4>
      <GetSaidasList />
    </div>
  );
};

export default Saida;

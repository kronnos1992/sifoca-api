import { LinearProgress } from '@mui/material';
import React, { useEffect, useState } from 'react';
import "../template.css";


const SuccessMessage = (props) => {
   const [isVisible, setIsVisible] = useState(true);
    const [progressValue, setProgressValue] = useState(0);
    useEffect(() => {
        const timeout = setTimeout(() => {
            setIsVisible(false);
        }, 3000); // 3000 milissegundos = 3 segundos
        const interval = setInterval(() => {
      // Atualiza o valor do progresso em incrementos de 5
      setProgressValue((prevValue) => {
        const newValue = prevValue + 33.3;
        // Certifique-se de que o valor não ultrapasse 100
        return newValue <= 100 ? newValue : 100;
      });
    }, 1000); // Intervalo de 1 segundo

        return () => {
            clearInterval(interval)
            clearTimeout(timeout);
        };
    }, []);

    const divStyle = {
        opacity: isVisible ? 1 : 0,
        transition: 'opacity 0.9s ease-in-out', // Você pode ajustar a duração da transição conforme necessário
    };

    return isVisible ? (
        <div style={divStyle} className={`alert alert-${props.variant || 'info'}`}>
            {props.children}
            <LinearProgress variant='determinate' color='info' value={progressValue} />
        </div>
    ) : null;

}
export default SuccessMessage;


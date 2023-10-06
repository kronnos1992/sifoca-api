import { LinearProgress, Typography } from '@mui/material';
import Box from '@mui/material/Box';
import * as React from 'react';


const LoadingMessage = (props) => {
    return (
        <Box sx={{ display: 'flex', alignItems: 'center' }}>
             <Box sx={{ marginLeft:"15rem", width: '70%', mr: 1 }}>
                 <LinearProgress variant="determinate" {...props} />
             </Box>
             <Box sx={{ minWidth: 35 }}>
                 <Typography variant="body2" color="text.secondary">{`${Math.round(
                     props.value,
                 )}%`}</Typography>
             </Box>
         </Box>
    );
}
export default LoadingMessage;

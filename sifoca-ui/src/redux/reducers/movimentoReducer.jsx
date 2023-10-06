import { movimentoTypes } from "../constants/movimentoTypes";


const initialState = {
    movimentos: []
}


export function getAllMovimentosReducer (state = initialState, action) { 
    switch (action.type) {

        case movimentoTypes.REQ_GET_MOVIMENTO:
            return {
                loading: true,
            };

        case movimentoTypes.SUCCESS_GET_MOVIMENTO:
            return {
                loading: false,
                state,
                movimentos: [...action.payload]
            };

        case movimentoTypes.FAIL_GET_MOVIMENTO:
            return {
                loading: false,
                error: [...action.payload]
            };

        default:
            return state

    }
};

// export function getCountryReducer (state = initialState, action) {
//     switch (action.type) {

//         case movimentoTypes.REQ_DETALHE_MOVIMENTO:
//             return {
//                 loading: true,
//             };

//         case movimentoTypes.SUCCESS_DETALHE_MOVIMENTO:
//             return {
//                 loading: false,
//                 countries: [...action.payload]
//             };

//         case movimentoTypes.FAIL_DETALHE_MOVIMENTO:
//             return {
//                 loading: false,
//                 error: [...action.payload]
//             }

//         default:
//             return state

//     }
// };

// export function insertCountryReducer (state = initialState, action) {
//     switch (action.type) {
//         case movimentoTypes.REQ_INSERT_MOVIMENTO:
//             return { loading: true }

//         case movimentoTypes.SUCCESS_INSERT_MOVIMENTO:
//             // state.push(action.payload)
//             return {
//                 ...state,
//                 success: true,
//                 loading: false,
//                 //countries: [...action.payload]
//             }

//         case movimentoTypes.FAIL_INSERT_MOVIMENTO:
//             return {
//                 loading: false,
//                 error: [...action.payload]
//             }

//         case movimentoTypes.RESET_INSERT_MOVIMENTO:
//             return {
//                 ...state,
//                 countries: []
//             }

//         default:
//             return state

//     }
// }

// export function updateCountryReducer (state = initialState, action) {
//     switch (action.type) {
//         case movimentoTypes.REQ_UPDATE_MOVIMENTO:
//             return { loading: true }

//         case movimentoTypes.SUCCESS_UPDATE_MOVIMENTO:
            
//             return {
//                 success: true,
//                 loading: false,
//                 countries: state.countries.map(x => x.id ===  action.payload.id ? action.payload : x)
//             };

//         case movimentoTypes.FAIL_UPDATE_MOVIMENTO:
//             return {
//                 loading: false,
//                 error: action.payload
//             }

//         case movimentoTypes.RESET_UPDATE_MOVIMENTO:
//             return {
//                 ...state,
//                 countries: []
//             }

//         default:
//             return state

//     }
// }

// export function deleteCountryReducer (state = initialState, action) {
//     switch (action.type) {
//         case movimentoTypes.REQ_DELETE_MOVIMENTO:
//             return { loading: true }

//         case movimentoTypes.SUCCESS_DELETE_MOVIMENTO:
//             return {
//                 loading: false,
//                 success: true
//             };

//         case movimentoTypes.FAIL_DELETE_MOVIMENTO:
//             return {
//                 loading: false,
//                 error: action.payload
//             }

//         case movimentoTypes.RESET_DELETE_MOVIMENTO:
//             return {
//                 ...state,
//                 countries: []
//             }

//         default:
//             return state

//     }
// }
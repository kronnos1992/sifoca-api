import axios from "axios";
import { movimentoTypes } from "../constants/movimentoTypes";

const baseUrl = process.env.REACT_APP_BASE_URL + "/movimentos/";

export const getAllMovimentos = async (dispatch) => {

    dispatch({
        type: movimentoTypes.REQ_GET_MOVIMENTO,
    });
    try {
        // api request
        const { data } = await axios.get(baseUrl);
        dispatch({
            type: movimentoTypes.SUCCESS_GET_MOVIMENTO,
            payload: data
        });
        
    } catch (error) {
        dispatch({
            type: movimentoTypes.FAIL_GET_MOVIMENTO,
            payload: error.message
        });
    }
}

// export const getCountryById = async (dispatch, id) => {

//     dispatch( reqOneCountry());
//     try {
//         // api request
//         const { data } = await axios.get(`${baseUrl}${id}`);
//         dispatch( oneCountry(data));
//     } catch (error) {
//         dispatch( errorOneCountry(error.response.message));
//     }
// }

// export const insertCountry = async (dispatch, country) => {
//     dispatch( reqInsertCountry());
//     try {
//         // api request
//         const { data } = await axios.post(baseUrl, country)
//         dispatch( insertCountry(data));
//     } catch (error) {
//         dispatch( errorInsertCountry(error.message));
//     }
// }

// export const updateCountry = async (dispatch, country) => {
//     try {
//         // api request
//         const { data } = await axios.put(`${baseUrl}${country.Id}`, country)
//         dispatch( updateCountry(data));
//     } catch (error) {
//         dispatch( errorUpdateCountry(error.message))
//     }
// }

// export const deleteCountry = async (dispatch, country) => {
//     dispatch( reqDeleteCountry());
//     try {
//         // api request
//         await axios.delete(`${baseUrl}${country.Id}`);
//         dispatch( deleteCountry(country));
//     } catch (error) {
//         dispatch( errorDeleteCountry(error.message))
//     }
// }
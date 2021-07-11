import logo from './logo.svg';
import './App.css';
import React, { useState } from 'react';
import { Form, TextField, SelectField, SubmitButton } from './FormElements';
import DynamicTable from './DynamicTable';
import * as Yup from 'yup';
import {
  Formik,
  Form as FormikForm
}
  from 'formik';

const FormSchema = Yup.object().shape({
  name: Yup.string()
    .required('Required'),
  damagecost: Yup.string()
    .required('Required'),
  claimtype: Yup.string()
    .required('Number of tickets is required'),
});

function App() {
  const [formData, setFormData] = useState({
    
  });

  const onSubmit = (values, { setSubmitting, resetForm, setStatus }) => {
    // console.log(values);
    setSubmitting(false);
  }

  return (
    <div className="App">
      <Formik
        enableReinitialize
        validationSchema={FormSchema}
        initialValues={formData}
        onSubmit={onSubmit}
      >
        {({ errors, values, touched, setValues }) => (
          <FormikForm className="needs-validation">

            <DynamicTable errors={errors}
              values={values}
              touched={touched}
              setValues={setValues}>

            </DynamicTable>
          </FormikForm>
        )}
      </Formik>
    </div>
  );
}


export default App;

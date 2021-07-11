import React from 'react';
import axios from 'axios';
import { Field } from 'formik';
import { Form, TextField, SelectField, SubmitButton } from './FormElements';

export default class DynamicTable extends React.Component {
  constructor(props, errors, values, touched, setValues) {
    super(props);
    this.axios = axios.create({
      headers: {
        'Content-Type': 'application/json', Accept: 'application/json',
        'Access-Control-Allow-Origin': 'http://localhost:3000',
        "Access-Control-Allow-Methods": "GET,PUT,POST,DELETE,PATCH,OPTIONS"
      },
      baseURL: 'http://localhost:5000',
    })
    this.errors = errors
    this.state = {
      id : "",
      name: "",
      damageCost: 0,
      year: 0,
      type: "",
      items: []
    }
    // console.log(this.axios.get('/Insurance'));
    this.axios.get('/Insurance').then((resp) => {
      // this.state.items = resp.data
    })
    // console.log(this.state)
    
  }

  updateName(event) {

    this.setState({
      name: event.target.value
    });
  }
  updateCost(event) {

    this.setState({
      damageCost: event.target.value
    });
  }
  updateYear(event) {
    // console.log(this.axios.get('/Insurance'));
    this.setState({
      year: event.target.value
    });
  }

  updateType(event) {
    // console.log(event.target.value)
    this.setState({
      type: event.target.value
    });
  }


  updateMessage(event) {
    // console.log(this.axios.get('/Insurance'));

    this.setState({
      message: event.target.value
    });
  }

  handleClick() {


    var items = this.state.items;
    console.log(items)
    // console.log(this.state)
    this.axios.post('/Insurance/', {
      name: items.name,
      damageCost: items.damageCost,
      year: items.year,
      type: items.type,
    });
      
    items.push({
      name: this.state.name,
      damageCost: this.state.damageCost,
      year: this.state.year,
      type: this.state.type
    });


    Array.from(document.querySelectorAll("input")).forEach(
      input => (input.value = "")
    );
    this.setState({
      items: items,
      message: ""
    });
  }

  // handleItemChanged(i, event) {
  //   var items = this.state.items;
  //   items[i] = event.target.value;

  //   this.setState({
  //     items: items
  //   });
  // }

  handleItemDeleted(i) {
    var items = this.state.items;

    items.splice(i, 1);

    this.setState({
      items: items
    });
  }

  renderRows() {
    var context = this;

    return this.state.items.map(function (o, i) {
      console.log(context.state.items)
      return (
        <tr key={"item-" + i}>
          <td>
            {o.name}
          </td>
          <td>
            {o.year}
          </td>
          <td>
            {o.damageCost}
          </td>
          <td>
            {o.type}
          </td>
          <td>
            <button
              onClick={context.handleItemDeleted.bind(context, i)}
            >
              Delete
            </button>
          </td>
        </tr>
      );
    });
  }

  render() {
    return (
      <div>
        <table className="">
          <thead>
            <tr>
              <th>
                Name
              </th>
              <th>
                DamageCost
              </th>
              <th>
                Year
              </th>
              <th>
                Type
              </th>
            </tr>
          </thead>
          <tbody>

            {this.renderRows()}
            <tr>
              <td>
                <TextField
                  name="name"
                  onChange={this.updateName.bind(this)}
                />
              </td>
              <td>
                <TextField
                  name="damagecost"
                  type='number'
                  onChange={this.updateCost.bind(this)}
                />
              </td>
              <td>
                <TextField
                  name="year"
                  onChange={this.updateYear.bind(this)}
                  type='number'
                  min={1000}
                  max={9999}
                />
              </td>
              <td>
              <Field name="claimtype">
                                    {({ field }) => (
                                        <select {...field} className={'form-control'+ (this.errors.type)} onChange={this.updateType.bind(this)}>
                                            <option value=""></option>
                                            {['Collision',"Grounding","Bad Weather","Fire"].map(i => 
                                                <option key={i} value={i}>{i}</option>
                                            )}
                                        </select>
                                    )}
                                    </Field>
                {/* <select id="lang" onChange={this.updateType.bind(this)} value={this.state.value}>
                  <option value="select">Select</option>
                  <option value="Collision">Collision</option>
                  <option value="Grounding">Grounding</option>
                  <option value="Bad Weather">Bad Weather</option>
                  <option value="Fire">Fire</option>
                </select> */}
                {/* <select
                  name="type"
                  onChange={this.updateType.bind(this)}
                  options={[
                    {
                      label: "Collision",
                      value: "Collision"
                    },
                    {
                      label: "Grounding",
                      value: "Grounding"
                    },
                    {
                      label: "Bad Weather",
                      value: "Bad Weather"
                    },
                    {
                      label: "Fire",
                      value: "Fire"
                    }
                  ]}
                /> */}
              </td>
              <td>
                <SubmitButton
                  title="Add a claim"
                  onClick={this.handleClick.bind(this)}
                >
                </SubmitButton>
              </td>

            </tr>
          </tbody>
        </table>


      </div>
    );
  }
}
